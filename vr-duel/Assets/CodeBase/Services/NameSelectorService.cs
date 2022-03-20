using System;
using System.Collections.Generic;
using CodeBase.Services.Progress;
using Newtonsoft.Json;

namespace CodeBase.Services
{
    public class NameSelectorService : INameSelectorService
    {
        private readonly IStorageService _storageService;

        private const string ProgressId = "Progress";

        public Action<string> OnSelect { get; set; }

        public NameSelectorService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public IEnumerable<string> GetSavedPlayersNames()
        {
            return GetProgressDictionary()?.Keys;
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