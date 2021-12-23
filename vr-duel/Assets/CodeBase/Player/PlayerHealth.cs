using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public PlayerVfx PlayerVfx;
        public float Current { get; set; } = 100f;
        public float Max { get; set; }

        public void Hit(float damage, Vector3 hitPoint, Vector3 direction)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            PlayerVfx.PlayDamageEffect(hitPoint, direction);
            Debug.Log($"Current health of '{gameObject.name}' is '{Current}'");
        }
    }
}