using System;
using System.Collections.Generic;

namespace CodeBase.Services
{
    public interface INameSelectorService : IService
    {
        Action<string> OnSelect { get; set; }
        IEnumerable<string> GetSavedPlayersNames();
    }
}