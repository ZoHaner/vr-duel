using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public static class XRInputUtilities
    {
        public static bool GetControllerInputValue(XRNode hand, InputFeatureUsage<bool> key)
        {
            var controller = TryGetDeviceForHand(hand);
            return TryGetControllerValue(controller, key);
        }

        public static Vector2 GetControllerInputValue(XRNode hand, InputFeatureUsage<Vector2> axis)
        {
            var controller = TryGetDeviceForHand(hand);
            return TryGetControllerValue(controller, axis);
        }

        public static bool TryGetControllerValue(InputDevice controller, InputFeatureUsage<bool> key)
        {
            if (controller.TryGetFeatureValue(key, out var value))
                return value;

            return false;
        }

        public static Vector2 TryGetControllerValue(InputDevice controller, InputFeatureUsage<Vector2> axis)
        {
            if (controller.TryGetFeatureValue(axis, out var value))
                return value;

            return Vector2.zero;
        }

        public static InputDevice TryGetDeviceForHand(XRNode handType)
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