using UnityEngine;
using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public class VRInputService : IInputService
    {
        private readonly Control<bool> _gunControl = new Control<bool>(XRNode.RightHand, CommonUsages.triggerButton);
        private readonly Control<bool> _menuControl = new Control<bool>(XRNode.LeftHand, CommonUsages.menuButton);
        private readonly Control<Vector2> _movingControl = new Control<Vector2>(XRNode.LeftHand, CommonUsages.primary2DAxis);
        
        private readonly XRInputSystem _xrInputSystem;

        public VRInputService()
        {
            _xrInputSystem = new XRInputSystem();
        }
        
        public Vector2 GetMoveAxis()
        {
            return _xrInputSystem.GetControllerInputValue(_movingControl.Hand, _movingControl.Button);
        }

        public bool IsAttackButtonPressed()
        {
            return _xrInputSystem.GetControllerInputValue(_gunControl.Hand, _gunControl.Button);
        }

        public bool IsMenuButtonPressed()
        {
            return _xrInputSystem.GetControllerInputValue(_menuControl.Hand, _menuControl.Button);
        }
    }
}