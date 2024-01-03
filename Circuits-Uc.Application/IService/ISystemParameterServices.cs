using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SystemParameterDTO;
using CircuitsUc.Application.DTOS.SystemParameterKeyDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IServices
{
    public interface ISystemParameterServices
    {
        Task<GeneralResponse<List<SystemParameter>>> GetAllAsync();
        Task<GeneralResponse<List<SystemParameterDto>>> GetAll(bool isEnglish);
        Task<GeneralResponse<SystemParameterDto>> GetByIdAsync(Guid Id,bool isEnglish);
        Task<GeneralResponse<Guid>> Add(SystemParameterInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(SystemParameterUpdateInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> DeleteByIdAsync(Guid Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        SystemParameter GetSystemParameterAsync(String code);
        Task<GeneralResponse<Object>> GetSystemParameterByKeyName(string KeyName, bool isEnglish);
        Task<GeneralResponse<Guid>> DeleteImage(Guid Id);
        Task<GeneralResponse<ContactInfo>> GetContactInfo();
    }
}
