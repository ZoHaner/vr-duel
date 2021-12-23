using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        // public PlayerVfx PlayerVfx;
        public float Current { get; set; } = 100f;
        public float Max { get; set; }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            // PlayerVfx.PlayDamageEffect();
            Debug.Log($"Current health of '{gameObject.name}' is '{Current}'");
        }
    }
}