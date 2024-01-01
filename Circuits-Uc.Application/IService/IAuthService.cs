using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Models.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IService
{
    public interface IAuthService
    {
        Task<GeneralResponse<AuthResponse>> Login(AuthRequest request);
        Task<GeneralResponse<ChangeUserPasswordResponse>> ChangePassword(ChangeUserPasswordRequest request);
        Task<bool> Logout(Guid UserID);

    }
}
