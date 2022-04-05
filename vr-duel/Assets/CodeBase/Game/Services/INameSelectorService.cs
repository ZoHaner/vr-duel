using System;

namespace CodeBase.Services
{
    public interface INameSelectorService : IService
    {
        Action<string> OnSelect { get; set; }
    }
}