using System;
using CodeBase.Player;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Behaviours.Guns
{
    public class GunShooting : MonoBehaviour
    {
        private const int Damage = 1;
        public Transform ShootPointTransform;
        public GunVfx GunVfx;

        private IInputEventService _inputEventService;

        public void Construct(IInputEventService inputEventService)
        {
            _inputEventService = inputEventService;
        }

        public void SubscribeEvents()
        {
            _inputEventService.AttackButtonPressed += Shoot;
        }

        public void OnDestroy()
        {
            UnsubscribeEvents();
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
