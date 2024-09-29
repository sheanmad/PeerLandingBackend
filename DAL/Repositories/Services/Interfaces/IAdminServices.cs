using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IAdminServices
    {
        Task<string> AddUser(ReqRegisterUserDto register);
        Task<List<ResUserDto>> GetAllUsers();
        Task<ResGetUserByIdDto> GetUserById(string Id);
        Task<string> UpdateUser(string userId, ReqUpdateUserDto reqUpdate);
        Task<string> Delete(string userId);
    }
}
