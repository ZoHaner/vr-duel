using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerAccountsService : IPlayerAccountsService
    {
        private const string AccountsKey = "Accounts";
        
        private IStorageService _storageService;

        public PlayerAccountsService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public void SaveNewAccount(UserAccount newAccount)
        {
            var accountList = GetAccountsList();
            if (AccountAlreadyExist(newAccount, accountList))
            {
                Debug.LogError($"Account with name {newAccount.Username} is already exist!");
                return;
            }
            
            accountList.Add(newAccount);
            
            var json = JsonConvert.SerializeObject(accountList);
 
            _storageService.WriteData(AccountsKey, json);
        }

        private bool AccountAlreadyExist(UserAccount newAccount, List<UserAccount> currentAccountList) => 
            currentAccountList.Any(a=>a.Username == newAccount.Username);

        public List<UserAccount> GetAccountsList()
        {
            var json = _storageService.ReadData(AccountsKey);
            if (string.IsNullOrEmpty(json))
                return null;

            var accountsList = JsonConvert.DeserializeObject<List<UserAccount>>(json);
            return accountsList;
        }
    }
}