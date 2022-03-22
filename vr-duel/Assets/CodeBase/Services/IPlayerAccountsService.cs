using System.Collections.Generic;
using CodeBase.Data;

namespace CodeBase.Services
{
    public interface IPlayerAccountsService : IService
    {
        void SaveNewAccount(UserAccount newAccount);
        List<UserAccount> GetAccountsList();
    }
}