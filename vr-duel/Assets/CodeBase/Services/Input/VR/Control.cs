using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public struct Control<T>
    {
        public XRNode Hand;
        public InputFeatureUsage<T> Button;

        public Control(XRNode hand, InputFeatureUsage<T> button)
        {
            Hand = hand;
            Button = button;
        }
    }
}