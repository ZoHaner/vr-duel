using UnityEngine;

namespace CodeBase.Behaviours.Player
{
    public class PlayerVfx : MonoBehaviour
    {
        public GameObject BloodVfxPrefab;
        
        public void PlayDamageEffect(Vector3 hitPoint, Vector3 direction)
        {
            var startRot = Quaternion.LookRotation(direction);
            Instantiate(BloodVfxPrefab, hitPoint, startRot);
        }
    }
}