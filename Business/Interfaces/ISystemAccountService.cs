using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

namespace Business.Interfaces
{
    public interface ISystemAccountService
    {
        SystemAccount? Authenticate(string email, string password);
        SystemAccount? GetUserById(short accountId);
        List<SystemAccount> GetAllUsers();
        void RegisterUser(SystemAccount user);
        void UpdateUser(SystemAccount user);
        void DeleteUser(short accountId);
        SystemAccount? GetUserByEmail(string email);
    }
}
