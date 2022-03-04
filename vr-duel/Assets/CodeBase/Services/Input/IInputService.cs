using System;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector3 HeadPosition { get; }
        Vector3 LeftHandPosition { get; }
        Vector3 RightHandPosition { get; }

        Action AttackButtonPressed { get; set; }
        bool IsAttackButtonPressed();
    }
}