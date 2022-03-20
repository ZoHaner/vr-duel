using UnityEngine;

namespace CodeBase.Services
{
    public class StorageService : IStorageService
    {
        public string ReadData(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void WriteData(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }
    }
}