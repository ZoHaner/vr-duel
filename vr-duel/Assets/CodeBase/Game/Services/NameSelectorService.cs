using System;

namespace CodeBase.Services
{
    public class NameSelectorService : INameSelectorService
    {
        public Action<string> OnSelect { get; set; }
    }
}