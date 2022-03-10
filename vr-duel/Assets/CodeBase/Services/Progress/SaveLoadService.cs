using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Services.Progress
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressId = "Progress";

        public PlayerProgress LoadProgressForPlayer(string username)
        {
            var json = PlayerPrefs.GetString(ProgressId);
            if (string.IsNullOrEmpty(json))
                return null;

            var progressDict = JsonConvert.DeserializeObject<Dictionary<string, PlayerProgress>>(json);
            
            if (progressDict.ContainsKey(username))
                return progressDict[username];
            else
                return null;
        }
    }
}