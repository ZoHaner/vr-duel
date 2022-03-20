using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services.Progress
{
    public class SaveLoadProgressService : ISaveLoadService
    {
        private readonly IStorageService _storageService;
        
        private const string ProgressId = "Progress";

        public SaveLoadProgressService(IStorageService storageService)
        {
            _storageService = storageService;
        }
        
        public PlayerProgress LoadProgressForPlayer(string username)
        {
            Dictionary<string, PlayerProgress> progressDict = GetProgressDictionary();
            
            if (progressDict == null) 
                return null;

            if (progressDict.ContainsKey(username))
                return progressDict[username];
            else
                return null;
        }

        public void SaveProgressForPlayer(string username, PlayerProgress progress)
        {
            Dictionary<string, PlayerProgress> progressDict = new Dictionary<string, PlayerProgress>();

            var json = PlayerPrefs.GetString(ProgressId);
            if (!string.IsNullOrEmpty(json))
            {
                progressDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerProgress>>(json);
            }

            progressDict[username] = progress;
            var updatedJson = JsonConvert.SerializeObject(progressDict);

            PlayerPrefs.SetString(ProgressId, updatedJson);
            PlayerPrefs.Save();
        }

        private Dictionary<string, PlayerProgress> GetProgressDictionary()
        {
            var json = _storageService.ReadData(ProgressId);
            if (string.IsNullOrEmpty(json))
                return null;

            var progressDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerProgress>>(json);
            return progressDict;
        }
    }
}