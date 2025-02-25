using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FunewsManagementContext _context;

        public SystemAccountRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public SystemAccount? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.SystemAccounts
                .FirstOrDefault(u => u.AccountEmail == email && u.AccountPassword == password);
        }

        public SystemAccount? GetUserById(short accountId)
        {
            return _context.SystemAccounts.Find(accountId);
        }

        public List<SystemAccount> GetAllUsers()
        {
            return _context.SystemAccounts.ToList();
        }

        public void AddUser(SystemAccount user)
        {
            short newAccountId = _context.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0;
            newAccountId++;
            user.AccountId = newAccountId;
            _context.SystemAccounts.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(SystemAccount user)
        {
            _context.SystemAccounts.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(short accountId)
        {
            var user = _context.SystemAccounts.Find(accountId);
            if (user != null)
            {
                _context.SystemAccounts.Remove(user);
                _context.SaveChanges();
            }
        }

        public SystemAccount? GetUserByEmail(string email)
        {
            return _context.SystemAccounts
                 .FirstOrDefault(u => u.AccountEmail == email);
        }
    }
}
