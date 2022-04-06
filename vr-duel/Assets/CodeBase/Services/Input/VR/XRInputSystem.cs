using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public class XRInputSystem
    {
        public bool GetControllerInputValue(XRNode hand, InputFeatureUsage<bool> key)
        {
            var controller = TryGetDeviceForHand(hand);
            return TryGetControllerValue(controller, key);
        }

        public Vector2 GetControllerInputValue(XRNode hand, InputFeatureUsage<Vector2> axis)
        {
            var controller = TryGetDeviceForHand(hand);
            return TryGetControllerValue(controller, axis);
        }

        private bool TryGetControllerValue(InputDevice controller, InputFeatureUsage<bool> key)
        {
            if (controller.TryGetFeatureValue(key, out var value))
                return value;

            return false;
        }
        
        private Vector2 TryGetControllerValue(InputDevice controller, InputFeatureUsage<Vector2> axis)
        {
            if (controller.TryGetFeatureValue(axis, out var value))
                return value;

            return Vector2.zero;
        }

        private InputDevice TryGetDeviceForHand(XRNode handType)
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(handType, devices);

            if(devices.Count == 1)
                return devices[0];

            if(devices.Count > 1) 
                Debug.LogError("Found more than one left hand!");

            return new InputDevice();
        }
    }
}