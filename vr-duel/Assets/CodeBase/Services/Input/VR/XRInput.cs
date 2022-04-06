using UnityEngine.XR;

namespace CodeBase.Services.Input.VR
{
    public abstract class XRInput<T> : IXRInput
    {
        protected XRNode Hand;
        protected InputFeatureUsage<T> DeviceElement;
        
        public T Value { get; protected set; }
        public abstract void UpdateValue();
    }
    
    public interface IXRInput
    {
        void UpdateValue();
    }
}