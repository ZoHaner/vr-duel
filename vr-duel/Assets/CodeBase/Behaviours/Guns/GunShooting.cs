using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Behaviours.Guns
{
    public class GunShooting : MonoBehaviour
    {
        private const float Damage = 1f;
        public Transform ShootPointTransform;
        
        public void Shoot()
        {
            PlayShootAnimation();
            TryApplyDamage();
        }

        private void TryApplyDamage()
        {
            if (TryRaycastPlayer(out RaycastHit hit))
            {
                var health = hit.transform.GetComponent<PlayerHealth>();
                health?.TakeDamage(Damage);
            }
        }

        private bool TryRaycastPlayer(out RaycastHit hit)
        {
            return Physics.Raycast(ShootPointTransform.position, ShootPointTransform.forward, out hit);
        }

        private void PlayShootAnimation()
        {
            
        }
    }
}
