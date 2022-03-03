using UnityEngine;

namespace CodeBase.Behaviours
{
    public class SpawnPoint : MonoBehaviour
    {
        private const float Radius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, Radius);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}