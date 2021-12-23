using UnityEngine;

namespace CodeBase.Behaviours.Guns
{
    public class GunActivator : MonoBehaviour
    {
        public void TryShoot()
        {
            var gunShooting = GetComponentInChildren<GunShooting>();
            
            if (gunShooting != null) 
                gunShooting.Shoot();
        }
    }
}