using System;
using UnityEngine;

namespace CodeBase.Services.Input.VR
{
    public class VRInputService : IInputService
    {
        public Vector3 HeadPosition { get; }
        public Vector3 LeftHandPosition { get; }
        public Vector3 RightHandPosition { get; }
        public Action AttackButtonPressed { get; set; }
        public bool IsAttackButtonPressed()
        {
            throw new NotImplementedException();
        }
    }
}