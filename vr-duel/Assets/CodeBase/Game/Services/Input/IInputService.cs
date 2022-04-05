using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 GetMoveAxis();
        bool IsAttackButtonPressed();
        bool IsMenuButtonPressed();
    }
}