using CodeBase.Player;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Behaviours.Guns
{
    public class GunShooting : MonoBehaviour
    {
        private const float Damage = 1f;
        public Transform ShootPointTransform;
        public GunVfx GunVfx;

        private IInputEventService _inputEventService;

        public void Construct(IInputEventService inputEventService)
        {
            _inputEventService = inputEventService;

            // SubscribeEvents();
        }

        // private void OnDestroy()
        // {
        //     UnsubscribeEvents();
        // }

        public void SubscribeEvents()
        {
            _inputEventService.AttackButtonPressed += Shoot;
        }

        public void UnsubscribeEvents()
        {
            _inputEventService.AttackButtonPressed -= Shoot;
        }

        public void Shoot()
        {
            Debug.LogError("Shoot");
            PlayShootAnimation();
            TryApplyDamage();
            GunVfx.PlayShootEffect(ShootPointTransform.position);
        }

        private void TryApplyDamage()
        {
            if (TryRaycastPlayer(out RaycastHit hit))
            {
                var health = hit.transform.GetComponent<PlayerHealth>();
                health?.Hit(Damage, hit.point, hit.normal);
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
