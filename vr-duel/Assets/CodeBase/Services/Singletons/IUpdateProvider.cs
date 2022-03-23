using System;

namespace CodeBase.Services.Singletons
{
    public interface IUpdateProvider : IService
    {
        void AddListener(Action action);
        void RemoveListener(Action action);
    }
}