using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService
    {
        Vector3 HeadPosition { get; }
        Vector3 LeftHandPosition { get; }
        Vector3 RightHandPosition { get; }

        bool IsAttackButtonPressed();
    }
}