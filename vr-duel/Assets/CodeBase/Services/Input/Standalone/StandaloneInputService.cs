using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class StandaloneInputService : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        public Vector2 GetMoveAxis()
        {
            return new Vector2(
                UnityEngine.Input.GetAxis(HorizontalAxisName),
                UnityEngine.Input.GetAxis(VerticalAxisName)
            ).normalized;
        }

        public bool IsAttackButtonPressed()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool IsMenuButtonPressed()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Escape);
        }
    }
}