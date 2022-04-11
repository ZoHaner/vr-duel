using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public class PositionSync : TransformComponentLerper<Vector3>
    {
        public PositionSync(Transform transform, ILerpable<Vector3> lerpable) : base(transform, lerpable)
        {
        }

        public override void ApplyLerp(float deltaTime) => 
            Transform.position = Lerpable.CalculateLerpValue(deltaTime);

        public override void SetNewTarget(Vector3 target) => 
            Lerpable.SetNewCurrentAndTarget(Transform.position, target);
    }
}