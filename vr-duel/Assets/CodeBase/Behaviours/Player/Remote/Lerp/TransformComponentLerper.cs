using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote.Lerp
{
    public abstract class TransformComponentLerper<T>
    {
        protected readonly Transform Transform;
        protected readonly ILerpable<T> Lerpable;

        protected TransformComponentLerper(Transform transform, ILerpable<T> lerpable)
        {
            Transform = transform;
            Lerpable = lerpable;
        }

        public abstract void ApplyLerp(float deltaTime);

        public abstract void SetNewTarget(T target);
    }
}