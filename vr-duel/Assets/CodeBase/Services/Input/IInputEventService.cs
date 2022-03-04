using System;

namespace CodeBase.Services.Input
{
    public interface IInputEventService : IService
    {
        bool InputChanged { get; set; }
        Action AttackButtonPressed { get; set; }
        bool IsAttackButtonPressed();
    }
}