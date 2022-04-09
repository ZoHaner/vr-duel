using UnityEngine;

namespace CodeBase.Extensions
{
    public static class GameObjectExtensions
    {
        public static void RotateToObjectAroundYAxis(this GameObject @base, GameObject target)
        {
            Vector3 targetPoint = target.transform.position;
            targetPoint.y = @base.transform.position.y;
            @base.transform.LookAt(targetPoint);
        }
    }
}