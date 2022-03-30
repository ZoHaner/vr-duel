using UnityEngine;

namespace CodeBase.Services.Input.VR
{
    public class VRInputService : IInputService
    {
        public Vector2 GetMoveAxis()
        {
            return Vector2.zero;
        }

        public bool IsAttackButtonPressed()
        {
            return false;
        }

        public bool IsMenuButtonPressed()
        {
            return false;
        }
    }
}