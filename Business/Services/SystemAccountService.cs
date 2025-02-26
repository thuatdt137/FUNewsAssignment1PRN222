using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business.Interfaces;
using DAL.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public SystemAccountService(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public SystemAccount? Authenticate(string email, string password)
        {
            // 1️⃣ Kiểm tra Admin từ appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            var adminRole = _configuration["AdminAccount:Role"];

            if (email == adminEmail && password == adminPassword)
            {
                return new SystemAccount
                {
                    AccountId = 0,
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole = 3,
                    Status = 1,
                };
            }

            return _accountRepository.GetUserByEmailAndPassword(email, password);
        }

        public SystemAccount? GetUserById(short accountId)
        {
            return _accountRepository.GetUserById(accountId);
        }

        public List<SystemAccount> GetAllUsers()
        {
            return _accountRepository.GetAllUsers();
        }

        public void RegisterUser(SystemAccount user)
        {
            _accountRepository.AddUser(user);
        }

        public void UpdateUser(SystemAccount user)
        {
            _accountRepository.UpdateUser(user);
        }

        public void DeleteUser(short accountId)
        {
            _accountRepository.DeleteUser(accountId);
        }
        public SystemAccount GetUserByEmail(string email)
        {
            var adminEmail = _configuration["AdminAccount:Email"];
            if (email.Equals(adminEmail))
            {
                SystemAccount admin = new SystemAccount();
                admin.AccountEmail = adminEmail;
                return admin;
            }
            return _accountRepository.GetUserByEmail(email);
        }
    }
}
