using System.Collections.Generic;
using CodeBase.Entities;
using CodeBase.ServiceLocator;

namespace CodeBase.Services
{
    public interface IPlayerAccountsService : IService
    {
        void SaveNewAccount(string username);
        List<UserAccount> GetAccountsList();
        bool AccountExist(string username);
        UserAccount GetAccountByUsername(string username);
    }
}