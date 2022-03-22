using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user,string password);
        Task<ServiceResponse<string>> Login(string userName, string password);
        Task<bool> UserExists(string userName);

    }
}
