using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SystemParameterKeyDTO;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = CircuitsUc.Domain.Entities.Document;

namespace CircuitsUc.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unit;

        const string WebSite = "";
        public DocumentService(IUnitOfWork unit) {
            _unit = unit;
          
        }
        public async Task<bool> Add(Document Input)
        {

            await _unit.Document.AddAsync(Input);
            return (await _unit.SaveAsync()) >= 1 ;
        }

        public void AddImage(Guid RefEntityID, Guid docCode, string enumVal, IFormFile Image)
        {
            #region Saving  Image
            string path = DocumentController.GetLocatedImagePath(Directory.GetCurrentDirectory(), enumVal, RefEntityID);
            string savingPath = Path.Combine(path, $"{docCode}{Path.GetExtension(Image.FileName)}");
            using (var stream = new FileStream(savingPath, FileMode.Create))
            {
                Image.CopyTo(stream);
            }
            #endregion
        }   
       


        public void AddFileBase64(Guid RefEntityID, Guid docCode, string enumVal, string FileName, string base64String)
        {
            FileManager.AddFileBase64(RefEntityID, docCode, enumVal, FileName, base64String);
        }
        public async Task<bool> Delete(Guid Id)
        {
            await _unit.Document.SoftDelete(Id);
            return (await _unit.SaveAsync()) >= 1;
        }
        public async Task<bool> DeleteRange(List<Guid> Id)
        {
            await _unit.Document.SoftDeleteRangeAsync(Id);
            return (await _unit.SaveAsync()) >= 1;
        }
        public async Task<List<Document>> GetAllAsync()
        {
           return await _unit.Document.All().ToListAsync();
        }

        public async Task<Document> GetByIdAsync(Guid Id)
        {
            return await _unit.Document.GetByIdAsync(Id);
        }
        public List<Document> GetDocumentByEntity(string? code, string EntityType) => _unit.Document.All().Where(z =>  (String.IsNullOrEmpty(code) || z.RefEntityID == code) && z.RefEntityTypeID == EntityType).ToList();
       
        public Document GetMainDocumentByEntity(string code, string EntityType)=> _unit.Document.All().AsNoTracking().Where(z => z.RefEntityID == code&& z.RefEntityTypeID == EntityType && z.IsMain).FirstOrDefault();
      

        public string  GetImagePath(Guid RefEntityID, Guid docCode, string enumValue, string FileName)
        {
            var WebSite = _unit.SystemParameter.All().FirstOrDefault(ex => ex.SettingKey.Equals(CommenEnum.SystemParameterKey.WebSiteUrl.ToString()));
            var webSiteUrl = WebSite == null ? null : JsonConvert.DeserializeObject<WebSiteUrl>(WebSite.SettingValueEN);
            var ImagePath = DocumentController.GetImage(RefEntityID, docCode, enumValue, FileName);
            ImagePath = WebSite == null ? ImagePath :
                webSiteUrl.Url + "/" + ImagePath;
            return ImagePath;

        }
        public string GetMainImagePath(Guid ID, string enumValue)
        {
            var doc = GetMainDocumentByEntity(ID.ToString(), enumValue);
            if (doc!=null){
                
                var webSiteUrl = WebSite == null ? null : WebSite;
                var ImagePath = DocumentController.GetImage(ID, doc.Id, enumValue, doc.FileName);
                ImagePath = WebSite == null ? ImagePath :
                    webSiteUrl + "/" + ImagePath;
                return ImagePath;
            }
            return null;

        }


        public void RemoveImage(Guid RefEntityID, Guid id, string FileName, string enumValue)
        {
            string p = DocumentController.GetLocatedImagePath(Directory.GetCurrentDirectory(), enumValue, RefEntityID);
            string path = Path.Combine(p, $"{id.ToString()}{Path.GetExtension(FileName)}");

            FileManager.DeleteSpecificFile(path);
            if (!Directory.EnumerateFileSystemEntries(p).Any())
            {
                FileManager.DeleteFolder(p);
               // Console.WriteLine("Directory is empty");
            }
        }

        public async Task<bool> PostImageAction(Guid Id, string EnumVal, string ImageBase64, string ImageName,bool IsMain)
        {
            #region Update IMG
           
                #region Saving  Image
                Document currentDoc = GetMainDocumentByEntity(Id.ToString(), EnumVal);

                if (currentDoc != null)
                {
                    await Delete(currentDoc.Id);
                    RemoveImage(Id, currentDoc.Id, currentDoc.FileName, EnumVal);
                }
                #endregion


                #region Adding Doc
                Document doc = new Document()
                {
                    RefEntityTypeID = EnumVal,
                    RefEntityID = Id.ToString(),
                    FileName = ImageName,
                    FileExtension = Path.GetExtension(ImageName),
                    IsMain = IsMain
                };

                if (!await Add(doc))
                {

                return false;

                }
            var EnumValString = Enum.GetName(typeof(CommenEnum.EntityType), EnumVal);
            if (EnumValString != null)
            {
                AddFileBase64(Id, doc.Id, EnumVal, ImageName, ImageBase64);
                return true;
            }
            #endregion
           return false;
            #endregion
        }

    }
}
