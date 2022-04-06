using UnityEngine;
using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public class XRInputAxis : XRInput<Vector2>
    {
        public XRInputAxis(XRNode hand, InputFeatureUsage<Vector2> deviceElement)
        {
            Hand = hand;
            DeviceElement = deviceElement;
        }
        
        public override void UpdateValue()
        {
            Value = XRInputUtilities.GetControllerInputValue(Hand, DeviceElement);
        }
    }
}