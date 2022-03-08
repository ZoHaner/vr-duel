using System;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public PlayerVfx PlayerVfx;
        public int Current { get; set; } = 3;
        public int Max { get; set; }

        public Action<int> HealthChanged;

        public void Hit(int damage, Vector3 hitPoint, Vector3 direction)
        {
            if (Current <= 0)
                return;

            ApplyDamage(damage);
            PlayerVfx.PlayDamageEffect(hitPoint, direction);
            Debug.Log($"Current health of '{gameObject.name}' is '{Current}'");
        }

        private void ApplyDamage(int damage)
        {
            Current -= damage;
            HealthChanged?.Invoke(Current);
        }
    }
}