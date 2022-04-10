using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public class QuaternionLerp : GenericLerp<Quaternion>
    {
        public QuaternionLerp(float lerpTime) : base(lerpTime)
        {
        }

        protected override Quaternion LerpFunction(Quaternion initialValue, Quaternion targetValue, float time)
        {
            return Quaternion.Lerp(initialValue, targetValue, time);
        }
    }
}