using System;
using CodeBase.ServiceLocator;

namespace CodeBase.Services
{
    public interface INameSelectorService : IService
    {
        Action<string> OnSelect { get; set; }
    }
}