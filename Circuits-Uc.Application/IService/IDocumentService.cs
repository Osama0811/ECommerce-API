using CircuitsUc.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IServices
{
    public interface IDocumentService
    {

        Task<bool> Add(Document Input);
        void AddImage(Guid RefEntityID, Guid docCode, string enumVal, IFormFile Image);
       
         Task<bool> Delete(Guid Id);
        Task<bool> DeleteRange(List<Guid> Id);
        Task<List<Document>> GetAllAsync();

        Task<Document> GetByIdAsync(Guid Id);


        List<Document> GetDocumentByEntity(string? code, string EntityType);
        Document GetMainDocumentByEntity(string code, string EntityType);
        string GetImagePath(Guid RefEntityID, Guid docCode, string enumValue, string FileName);
        string GetMainImagePath(Guid ID, string enumValue);
        void RemoveImage(Guid RefEntityID, Guid id, string FileName, string enumValue);



        void AddFileBase64(Guid RefEntityID, Guid docCode, string enumVal, string FileName, string base64String);


    }
}
