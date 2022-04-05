using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Entities;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services
{
    public class PlayerAccountsService : IPlayerAccountsService
    {
        private const string AccountsKey = "Accounts";

        private readonly IStorageService _storageService;
        private bool _hasCache;
        private List<UserAccount> _cachedList;

        public PlayerAccountsService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public void SaveNewAccount(string username)
        {
            if (AccountExist(username))
            {
                Debug.LogError($"Account with name {username} is already exist!");
                return;
            }

            var accountList = GetAccountsList();
            accountList.Add(CreateAccount(username));
            var json = JsonConvert.SerializeObject(accountList);

            _storageService.WriteData(AccountsKey, json);
            _hasCache = false;
        }

        public List<UserAccount> GetAccountsList()
        {
            if (_hasCache)
            {
                return _cachedList;
            }

            var json = _storageService.ReadData(AccountsKey);

            List<UserAccount> accountsList = string.IsNullOrEmpty(json) ? new List<UserAccount>() : JsonConvert.DeserializeObject<List<UserAccount>>(json);

            CacheList(accountsList);
            return accountsList;
        }

        public bool AccountExist(string username)
        {
            var accountList = GetAccountsList();
            return accountList.Any(a => a.Username == username);
        }

        public UserAccount GetAccountByUsername(string username)
        {
            var accountList = GetAccountsList();
            return accountList.FirstOrDefault(a => a.Username == username);
        }

        private UserAccount CreateAccount(string username) =>
            new UserAccount(Guid.NewGuid().ToString(), username);

        private void CacheList(List<UserAccount> accountsList)
        {
            _cachedList = accountsList;
            _hasCache = true;
        }
    }
}