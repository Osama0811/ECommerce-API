using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IServices
{
    public interface ISecurityUserService
    {
        

        Task<GeneralResponse<List<SecurityUserDto>>> GetAll();
        Task<GeneralResponse<SecurityUserDto>> GetByIdAsync(Guid Id);
        Task<GeneralResponse<Guid>> Add(SecurityUserInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(SecurityUserUpdateInput Input, Guid UserId);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);

        //bool CheckUser(SecurityUser SecurityUser, out string message);


        SecurityUser GetActiveUserById(Guid UserID);
    }
}
