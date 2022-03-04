using System;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public interface IUpdateProvider : IService
    {
        void AddListener(Action action);
        void RemoveListener(Action action);
    }
}