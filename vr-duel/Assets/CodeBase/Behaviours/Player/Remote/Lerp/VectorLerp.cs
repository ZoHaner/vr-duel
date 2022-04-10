using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public class VectorLerp : GenericLerp<Vector3>
    {
        public VectorLerp(float lerpTime) : base(lerpTime)
        {
        }

        protected override Vector3 LerpFunction(Vector3 initialValue, Vector3 targetValue, float time)
        {
            return Vector3.Lerp(initialValue, targetValue, time);
        }
    }
}