using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public class RotationSync : TransformComponentLerper<Quaternion>
    {
        public RotationSync(Transform transform, ILerpable<Quaternion> lerpable) : base(transform, lerpable)
        {
        }

        public override void ApplyLerp(float deltaTime) => 
            Transform.rotation = Lerpable.CalculateLerpValue(deltaTime);

        public override void SetNewTarget(Quaternion target) => 
            Lerpable.SetNewCurrentAndTarget(Transform.rotation, target);
    }
}