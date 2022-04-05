using System;
using CodeBase.ServiceLocator;

namespace CodeBase.Services.Singletons
{
    public interface IUpdateProvider : IService
    {
        void AddListener(Action action);
        void RemoveListener(Action action);
    }
}