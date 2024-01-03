using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SystemParameterDTO;
using CircuitsUc.Application.DTOS.SystemParameterKeyDTO;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Services
{
    public class SystemParameterService : ISystemParameterServices
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _UnitOfWork;

        private readonly IStringLocalizer<GeneralMessages> _localization;
        private readonly IDocumentService _documentService;

        public SystemParameterService(IUnitOfWork UnitOfWork, IMapper Mapper
            , IStringLocalizer<GeneralMessages> localization,IDocumentService documentService)
        {
            _mapper = Mapper;
            _UnitOfWork = UnitOfWork;
            _localization = localization;
            _documentService = documentService;
        }

        public async Task<GeneralResponse<Guid>> Add(SystemParameterInput Input,Guid UserId)
        {
            
            var results = false;
            var systemParameter = _mapper.Map<SystemParameterInput, SystemParameter>(Input);

            systemParameter.CreatedBy =UserId;
            #region CheckName
            if (checkNameExist(systemParameter))
            {
                return new GeneralResponse<Guid>(_localization["NameExist"], System.Net.HttpStatusCode.BadRequest);
            }
            #endregion
            await _UnitOfWork.SystemParameter.AddAsync(systemParameter);

            if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
            {
                #region Adding Doc
                Document doc = new Document()
                {
                    RefEntityTypeID = Convert.ToInt32(CommenEnum.EntityType.SystemParameter).ToString(),
                    RefEntityID = systemParameter.Id.ToString(),
                    FileName = Input.FileName,
                    FileExtension = Path.GetExtension(Input.FileName),
                    IsMain = true,
                    CreatedBy = UserId
                };
                results = (await _documentService.Add(doc));
                if (!results)
                {

                    return new GeneralResponse<Guid>(_localization["ErrorInSave"], System.Net.HttpStatusCode.BadRequest);

                }
                #endregion

                #region Saving  Image
                _documentService.AddFileBase64(systemParameter.Id, doc.Id, CommenEnum.EntityType.SystemParameter.ToString(), Input.FileName, Input.ImageBase64);

                #endregion
            }
            else
            {
                var val = _UnitOfWork.Save();
                results = val >= 1 ? true : false;
            }
            
            return results == true ? new GeneralResponse<Guid>(systemParameter.Id, _localization["AddedSuccessfully"]) :
                 new GeneralResponse<Guid>(_localization["ErrorInSave"], System.Net.HttpStatusCode.BadRequest);

        }
        public async Task<GeneralResponse<Guid>> DeleteByIdAsync(Guid Id)
        {

            SystemParameter systemParameter = await _UnitOfWork.SystemParameter.GetByIdAsync(Id);
            if (systemParameter == null)
            {
                return new GeneralResponse<Guid>(_localization["NotFound"], System.Net.HttpStatusCode.BadRequest);
            }
           
            await _UnitOfWork.SystemParameter.DeleteAsync(systemParameter);
            var results = _UnitOfWork.Save();
           
            return results >= 1 ? new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"]) :
                 new GeneralResponse<Guid>(_localization["ErrorInDelete"], System.Net.HttpStatusCode.BadRequest);
        }
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id)
        {

            await _UnitOfWork.SystemParameter.SoftDeleteRangeAsync(Id);

            var results = _UnitOfWork.Save();

            return results >= 1 ? new GeneralResponse<List<Guid>>(Id,_localization["DeletedSuccesfully"]) :
                 new GeneralResponse<List<Guid>>(_localization["ErrorInDelete"], System.Net.HttpStatusCode.BadRequest);
        }
        public async Task<GeneralResponse<Guid>> SoftDelete(Guid Id)
        {

            SystemParameter systemParameter = await _UnitOfWork.SystemParameter.GetByIdAsync(Id);
            if (systemParameter == null)
            {
                return new GeneralResponse<Guid>(_localization["NotFound"], System.Net.HttpStatusCode.BadRequest);
            }
           
            await _UnitOfWork.SystemParameter.SoftDelete(Id);
            var results = _UnitOfWork.Save();
         
            return results >= 1 ? new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"]) :
                 new GeneralResponse<Guid>(_localization["ErrorInDelete"], System.Net.HttpStatusCode.BadRequest);
        }


       

        public async Task<GeneralResponse<List<SystemParameterDto>>> GetAll(bool isEnglish)
        {
            
            var results = await _UnitOfWork.SystemParameter.All()
            .Select(x => new SystemParameterDto
            {
                Id = x.Id,
                //SettingValueAR = x.SettingValueAR,
                SettingKey = x.SettingKey,
                IsSystemKey = x.IsSystemKey,
                //SettingValueEN = x.SettingValueEN,
                //SettingValue = isEnglish ? x.SettingValueEN : x.SettingValueAR
            }).ToListAsync();

            return new GeneralResponse<List<SystemParameterDto>>(results, results.Count().ToString(), results.Count());
        }

        public async Task<GeneralResponse<List<SystemParameter>>> GetAllAsync()
        {
            var results = await _UnitOfWork.SystemParameter.All().ToListAsync();

            return new GeneralResponse<List<SystemParameter>>(results, results.Count().ToString(), results.Count());
        }

        public async Task<GeneralResponse<SystemParameterDto>> GetByIdAsync(Guid Id,bool isEnglish)
        {
            var results =  _UnitOfWork.SystemParameter.All().ToList().Where(z=>z.Id==Id)
             .Select(x => new SystemParameterDto
             {
                 Id = x.Id,
                 SettingValueAR = GetSettingValue(x.SettingValueAR, x.SettingKey,x.Id),
                 SettingKey = x.SettingKey,
                 IsSystemKey = x.IsSystemKey,
                 SettingValueEN = GetSettingValue(x.SettingValueEN, x.SettingKey, x.Id),
                 SettingValue =isEnglish? GetSettingValue(x.SettingValueEN,x.SettingKey, x.Id) : GetSettingValue(x.SettingValueAR, x.SettingKey, x.Id)
             }).FirstOrDefault();
          
            return new GeneralResponse<SystemParameterDto>(results);
        }

        public async Task<GeneralResponse<ContactInfo>> GetContactInfo()
        {
            try
            {
                var ContactInfo = _UnitOfWork.SystemParameter.All().Where(x => x.SettingKey == CommenEnum.SystemParameterKey.ContactInfo.ToString()).FirstOrDefault();
                var obj = JsonConvert.DeserializeObject<ContactInfo>(ContactInfo.SettingValueEN);

                return new GeneralResponse<ContactInfo>(obj);
            }
            catch (Exception ex) {
                return new GeneralResponse<ContactInfo>("There Are Exception "+ex, System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<GeneralResponse<Guid>> Update(SystemParameterUpdateInput Input,Guid UserId)
        {
            SystemParameter systemParameter = await _UnitOfWork.SystemParameter.GetByIdAsync(Input.Id);

            systemParameter = _mapper.Map<SystemParameterUpdateInput, SystemParameter>(Input, systemParameter);
            systemParameter.UpdatedBy = UserId;
            #region CheckName
            if (checkNameExist(systemParameter))
            {
                return new GeneralResponse<Guid>(_localization["NameExist"], System.Net.HttpStatusCode.BadRequest);
            }
            #endregion
            await _UnitOfWork.SystemParameter.UpdateAsync(systemParameter);

            int _enumVal = (int)CommenEnum.EntityType.SystemParameter;
            if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
            {
                #region Saving SystemParameter Image
                Document currentDoc = _documentService.GetMainDocumentByEntity(Input.Id.ToString(), _enumVal.ToString());

                if (currentDoc != null)
                {
                    await _documentService.Delete(currentDoc.Id);
                    _documentService.RemoveImage(Input.Id, currentDoc.Id, currentDoc.FileName, CommenEnum.EntityType.SystemParameter.ToString());
                }
                #endregion

            }
            if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
            {
                #region Adding Doc
                Document doc = new Document()
                {
                    RefEntityTypeID = Convert.ToInt32(CommenEnum.EntityType.SystemParameter).ToString(),
                    RefEntityID = Input.Id.ToString(),
                    FileName = Input.FileName,
                    FileExtension = Path.GetExtension(Input.FileName),
                    IsMain = true
                };
                var result = (await _documentService.Add(doc));
                if (!result)
                {

                    return new GeneralResponse<Guid>(_localization["ErrorInUpdated"], System.Net.HttpStatusCode.BadRequest);

                }
                #endregion
                #region Saving  Image
                _documentService.AddFileBase64(Input.Id, doc.Id, CommenEnum.EntityType.SystemParameter.ToString(), Input.FileName, Input.ImageBase64);
                return result == true ? new GeneralResponse<Guid>(systemParameter.Id, _localization["updatedSuccessfully"]) :
               new GeneralResponse<Guid>(_localization["ErrorInEdit"], System.Net.HttpStatusCode.BadRequest);

                #endregion
            }



            var results = _UnitOfWork.Save();

            return results >= 1 ? new GeneralResponse<Guid>(systemParameter.Id, _localization["updatedSuccessfully"]) :
                new GeneralResponse<Guid>(_localization["ErrorInEdit"], System.Net.HttpStatusCode.BadRequest);
        }


        public  SystemParameter GetSystemParameterAsync(String code)
        {

            return  _UnitOfWork.SystemParameter.All().FirstOrDefault(ex => ex.SettingKey.Equals(code));
           
        }

        public async Task<GeneralResponse<Object>> GetSystemParameterByKeyName(string KeyName, bool isEnglish)
        {
            var results =  _UnitOfWork.SystemParameter.All().ToList()
                .Where(ex => ex.IsDeleted != true && ex.SettingKey == KeyName)
             .Select(x => new 
             {

                 Id = x.Id,
                 Name = isEnglish ? GetSettingValue(x.SettingValueEN, x.SettingKey, x.Id) : GetSettingValue(x.SettingValueAR, x.SettingKey, x.Id),
             }).FirstOrDefault();
            return new GeneralResponse<Object>(results, _localization["Succes"]);

        }
        public async Task<GeneralResponse<Guid>> DeleteImage(Guid Id)
        {
            var _enumVal = (int)CommenEnum.EntityType.SystemParameter;
            Document currentDoc = _documentService.GetMainDocumentByEntity(Id.ToString(), _enumVal.ToString());

            if (currentDoc == null)
            {
                return new GeneralResponse<Guid>(Id, _localization["ImageNotFound"]);
            }

            await _documentService.Delete(currentDoc.Id);
            _documentService.RemoveImage(Id, currentDoc.Id, currentDoc.FileName, CommenEnum.EntityType.SystemParameter.ToString());

            return new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"]);
        }
        public bool checkNameExist(SystemParameter Input)
        {

            if (_UnitOfWork.SystemParameter.All().Where(x => Input.SettingKey != null && x.SettingKey.Equals(Input.SettingKey)  && (Input.Id == null || Input.Id == Guid.Empty || Input.Id != x.Id)).FirstOrDefault() != null)

            { return true; }
            return false;
        }
        public object GetSettingValue(string SettingValue,string SettingKey,Guid Id)
        {
            if (SettingKey == CommenEnum.SystemParameterKey.ContactEmailConfiguration.ToString())
            { return JsonConvert.DeserializeObject<ContactEmailConfiguration>(SettingValue); }
            
            else if(SettingKey == CommenEnum.SystemParameterKey.ContactInfo.ToString())
            { return JsonConvert.DeserializeObject<ContactInfo>(SettingValue);
            }
            else if (SettingKey == CommenEnum.SystemParameterKey.AboutApp.ToString())
            { return JsonConvert.DeserializeObject<AboutApp>(SettingValue); }

            else if (SettingKey == CommenEnum.SystemParameterKey.InviteApp.ToString())
            { return JsonConvert.DeserializeObject<InviteApp>(SettingValue); }

            else if (SettingKey == CommenEnum.SystemParameterKey.WebSiteUrl.ToString())
            { var webSiteUrl=JsonConvert.DeserializeObject<WebSiteUrl>(SettingValue);

                int _enumVal = (int)CommenEnum.EntityType.SystemParameter;
                Document currentDoc = _documentService.GetMainDocumentByEntity(Id.ToString(), _enumVal.ToString());
                if (currentDoc != null)
                {
                    webSiteUrl.ImagePath = _documentService.GetImagePath
                        (Id, currentDoc.Id,
                        CommenEnum.EntityType.SystemParameter.ToString(), currentDoc.FileName);
                    webSiteUrl.FileName = currentDoc.FileName;
                }
                return webSiteUrl;
            }

             return SettingValue; 

        }
    }
}
