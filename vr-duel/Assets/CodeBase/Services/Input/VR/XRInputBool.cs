using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public class XRInputBool : XRInput<bool>
    {
        private readonly PressType _pressType;

        private bool _isPressed;
        private bool _wasPressed;

        public XRInputBool(XRNode hand, InputFeatureUsage<bool> deviceElement, PressType pressType)
        {
            Hand = hand;
            DeviceElement = deviceElement;
            _pressType = pressType;
        }

        public override void UpdateValue()
        {
            _isPressed = XRInputUtilities.GetControllerInputValue(Hand, DeviceElement);
            
            switch (_pressType)
            {
                case PressType.Continuous: Value = _isPressed; break;
                case PressType.Begin: Value = _isPressed && !_wasPressed; break;
                case PressType.End: Value = !_isPressed && _wasPressed; break;
            }
            
            _wasPressed = _isPressed;
        }
    }
}