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
    }
}