using UnityEngine;
using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public class VRInputService : IInputService
    {
        private readonly XRInputBool _gunControl1 = new XRInputBool(XRNode.RightHand, CommonUsages.triggerButton, PressType.Begin);
        private readonly XRInputBool _menuControl1 = new XRInputBool(XRNode.LeftHand, CommonUsages.menuButton, PressType.Begin);
        private readonly XRInputAxis _movingControl1 = new XRInputAxis(XRNode.LeftHand, CommonUsages.primary2DAxis);
        
        private readonly XRInputUpdater _xrInputUpdater;

        public VRInputService(XRInputUpdater xrInputUpdater)
        {
            _xrInputUpdater = xrInputUpdater;
        }

        public void Initialize()
        {
            _xrInputUpdater.AddBinding(_gunControl1);
            _xrInputUpdater.AddBinding(_menuControl1);
            _xrInputUpdater.AddBinding(_movingControl1);
        }

        public bool IsAttackButtonPressed()
        {
            return _gunControl1.Value;
        }

        public bool IsMenuButtonPressed()
        {
            return _menuControl1.Value;
        }

        public Vector2 GetMoveAxis()
        {
            return _movingControl1.Value;
        }
    }
}