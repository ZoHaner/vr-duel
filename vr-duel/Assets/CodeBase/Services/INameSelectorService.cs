using System;
using CodeBase.Services;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public interface INameSelectorService : IService
    {
        Action<string> OnSelect { get; set; }
    }
}