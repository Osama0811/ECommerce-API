using CircuitsUc.Application.Communications;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using Microsoft.Extensions.Localization;
using CircuitsUc.Application.Helpers;
using static CircuitsUc.Application.Helpers.CommenEnum;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using Newtonsoft.Json.Linq;


namespace CircuitsUc.Application.Services
{
    public class SecurityUserService : ISecurityUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IDocumentService _documentService;
        private readonly IStringLocalizer<GeneralMessages> _localization;
    

        public SecurityUserService(IUnitOfWork unitOfWork, IMapper Mapper,IStringLocalizer<GeneralMessages> localization,IDocumentService documentService)
        {
            _localization = localization;
            _mapper=Mapper;
            _unit = unitOfWork;
            _documentService=documentService;
        }

        public async Task<GeneralResponse<SecurityUserDto>> GetByIdAsync(Guid Id)
        {
            var results =  _unit.SecurityUser.All().Where(x=>x.Id==Id).Include(x => x.CreatedByUser)
               .Include(x => x.UpdatedByUser).ToList()
                .Select(x => new SecurityUserDto
                {
                    UserName = x.UserName,
                    Id = x.Id,
                    Phone = x.Phone,
                    AlternativePhone = x.AlternativePhone,
                    Email = x.Email,
                    PreferedContact = x.PreferedContact,
                    RoleId = x.RoleId,
                    IsActive = x.IsActive,
                    IsOnline = x.IsOnline,
                    LastLoginDate = x.LastLoginDate,
                    UpdatedBynamed = x.UpdatedByUser.UserName,
                    CreatedBynamed = x.CreatedByUser.UserName,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    CreationDate = x.CreationDate,
                    UpdatedDate = x.UpdatedDate

                }).FirstOrDefault();

            return new GeneralResponse<SecurityUserDto>(results, _localization["Succes"].Value);
        }
        public async Task<GeneralResponse<List<SecurityUserDto>>> GetAll()
        {
            var results =  _unit.SecurityUser.All().Include(x=>x.CreatedByUser)
                .Include(x=>x.UpdatedByUser).ToList()
                 .Select(x => new SecurityUserDto
                 {
                    UserName = x.UserName,
                      Id= x.Id,
                      Phone=x.Phone,
                     AlternativePhone=x.AlternativePhone,
                     Email = x.Email,
                     PreferedContact =x.PreferedContact,
                     RoleId=x.RoleId,
                     IsActive=x.IsActive,
                     IsOnline=x.IsOnline,
                     LastLoginDate=x.LastLoginDate,
                     UpdatedBynamed=x.UpdatedByUser.UserName,
                     CreatedBynamed = x.CreatedByUser.UserName,
                     CreatedBy=x.CreatedBy,
                     UpdatedBy=x.UpdatedBy,
                     CreationDate=x.CreationDate,
                     UpdatedDate=x.UpdatedDate

                 }).ToList();
            return new GeneralResponse<List<SecurityUserDto>>(results, results.Count().ToString());
        }

       
        public async Task<GeneralResponse<Guid>> Add(SecurityUserInput Input,Guid UserId)
        {

            try
            {
                
                bool Succes=true;
                var SecurityUser = _mapper.Map<SecurityUserInput, SecurityUser>(Input);
                SecurityUser.CreatedBy=UserId;
                SecurityUser.CreationDate=DateTime.Now;
                SecurityUser.Password= WebUiUtility.Encrypt(Input.Password);
                #region CheckEmail&Phone&AlterPhone
                if (!CheckUser(SecurityUser,out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                SecurityUser.IsOnline = true;
                await _unit.SecurityUser.AddAsync(SecurityUser);
                #region Add IMG
                if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    #region Adding Doc
                    Document doc = new Document()
                    {
                        RefEntityTypeID = Input.RoleId.ToString(),
                        RefEntityID = SecurityUser.Id.ToString(),
                        FileName = Input.FileName,
                        FileExtension = Path.GetExtension(Input.FileName),
                        IsMain = true,
                        CreatedBy = UserId
                    };
                     Succes = (await _documentService.Add(doc));
                    if (!Succes)
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                    #endregion

                    #region Saving  Image
                    _documentService.AddFileBase64(SecurityUser.Id, doc.Id, SecurityUser.RoleId.ToString(), Input.FileName, Input.ImageBase64);

                    #endregion
                }
                else
                {
                    Succes = _unit.Save()>=1?true:false;
                }
                #endregion
                //var results =Succes? 1: 0;
           
                return Succes ? new GeneralResponse<Guid>(SecurityUser.Id, _localization["AddedSuccessfully"].Value) :
                     new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {

                return  new GeneralResponse<Guid>(ex.Message +"-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }
        public async Task<GeneralResponse<Guid>> Update(SecurityUserUpdateInput Input, Guid UserId)
        {

            try
            {

                SecurityUser securityUser = await _unit.SecurityUser.GetByIdAsync(Input.Id);

            securityUser = _mapper.Map<SecurityUserUpdateInput, SecurityUser>(Input,securityUser);
            securityUser.UpdatedBy = UserId;
            securityUser.UpdatedDate = DateTime.Now;
            #region CheckEmail&Phone&AlterPhone
            if (!CheckUser(securityUser, out string message))
            {
                return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
            }


            #endregion

            #region Update IMG
            int _enumVal = securityUser.RoleId;
            if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
            {
                #region Saving  Image
                Document currentDoc = _documentService.GetMainDocumentByEntity(Input.Id.ToString(), _enumVal.ToString());

                if (currentDoc != null)
                {
                    await _documentService.Delete(currentDoc.Id);
                    _documentService.RemoveImage(Input.Id, currentDoc.Id, currentDoc.FileName, _enumVal.ToString());
                }
                #endregion


                #region Adding Doc
                Document doc = new Document()
                {
                    RefEntityTypeID = _enumVal.ToString(),
                    RefEntityID = Input.Id.ToString(),
                    FileName = Input.FileName,
                    FileExtension = Path.GetExtension(Input.FileName),
                    IsMain = true
                };

                if (!await _documentService.Add(doc))
                {

                    return new GeneralResponse<Guid>(_localization["ErrorInUpdated"].Value, System.Net.HttpStatusCode.BadRequest);

                }
                
                    _documentService.AddFileBase64(Input.Id, doc.Id, _enumVal.ToString(), Input.FileName, Input.ImageBase64);

                    


                
                #endregion

            }


            #endregion
            
            await _unit.SecurityUser.UpdateAsync(securityUser);
            var results = await _unit.SaveAsync();
         
            return results >= 1 ? new GeneralResponse<Guid>(securityUser.Id, _localization["updatedSuccessfully"].Value) :
                new GeneralResponse<Guid>(_localization["ErrorInEdit"].Value, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }
        public async Task<GeneralResponse<List<Guid>>>SoftRangeDelete(List<Guid> Id)
        {

            await _unit.SecurityUser.SoftDeleteRangeAsync(Id);

            var results = _unit.Save();

            return results >= 1 ? new GeneralResponse <List<Guid>>(Id,_localization["DeletedSuccesfully"].Value) :
                 new GeneralResponse<List<Guid>>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);
        }
        public async Task<GeneralResponse<Guid>> SoftDelete(Guid Id)
        {
          
            await _unit.SecurityUser.SoftDelete(Id);
            var results = await _unit.SaveAsync();
         
            return results >= 1 ? new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"].Value) :
                new GeneralResponse<Guid>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);
       
        }
        public bool checkEmailExist(string Email,Guid? id)
        {
            if( _unit.SecurityUser.All().Where(x=> (Email != null && x.Email.Equals(Email))&&(id == null || id ==Guid.Empty || id != x.Id)).FirstOrDefault() != null)
            { return true; }
            return false;
        }
        public bool checkPhoneExist(string? Phone,Guid? id)
        {
           
            if (_unit.SecurityUser.All().Where(x => (Phone != null && x.Phone.Equals(Phone)) && (id == null || id == Guid.Empty || id != x.Id)).FirstOrDefault() != null)
                { return true; }
            return false;
        }
        public bool checkAlterPhoneExist(string? AlternativePhone, Guid? id)
        {
           
            if (_unit.SecurityUser.All().Where(x => (AlternativePhone!=""&&AlternativePhone !=null&& x.AlternativePhone.Equals(AlternativePhone)) && (id == null || id == Guid.Empty || id != x.Id)).FirstOrDefault() != null)

                { return true; }
            return false;
        }
       
        public bool CheckUser(SecurityUser SecurityUser,out string message)
        {
            #region CheckEmail&Phone&AlterPhone
            if (checkEmailExist(SecurityUser.Email, SecurityUser.Id) && checkPhoneExist(SecurityUser.Phone, SecurityUser.Id) && checkAlterPhoneExist(SecurityUser.AlternativePhone, SecurityUser.Id))
            {
                message = "EmailPhoneAlterPhone";
                return false;
            }
            else if (checkEmailExist(SecurityUser.Email, SecurityUser.Id) && checkPhoneExist(SecurityUser.Phone , SecurityUser.Id))
            {
                message = "EmailPhone";
                return false;
            }
            else if (checkEmailExist(SecurityUser.Email, SecurityUser.Id) && checkAlterPhoneExist(SecurityUser.AlternativePhone , SecurityUser.Id))
            {
                message = "EmailAlterPhone";
                return false;
            }
            else if (checkPhoneExist(SecurityUser.Phone, SecurityUser.Id) && checkAlterPhoneExist(SecurityUser.AlternativePhone , SecurityUser.Id))
            {
                message = "PhoneAlterPhone";
                return false;
            }
            else if (checkEmailExist(SecurityUser.Email, SecurityUser.Id))
            {
                message = "CheckEmail";
                return false;
            }
            else if (checkPhoneExist(SecurityUser.Phone, SecurityUser.Id))
            {
                message = "CheckPhone";
                return false;
            }
            else if (checkAlterPhoneExist(SecurityUser.AlternativePhone, SecurityUser.Id))
            {
                message = "CheckAlterPhone";
                return false;
            }
            else
            {
                message = "";
                return true;
            }
            #endregion
        }

        public SecurityUser GetActiveUserById(Guid UserID)
        {
            var User = _unit.SecurityUser.All().Where(ex => ex.Id == UserID&&ex.IsOnline).FirstOrDefault();
            return User;
        }
    }
}
