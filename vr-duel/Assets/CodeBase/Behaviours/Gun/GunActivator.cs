using UnityEngine;

namespace CodeBase.Behaviours.Gun
{
    public class GunActivator : MonoBehaviour
    {
        private GunShooting _gunObject;
        private GunShooting GunObject
        {
            get
            {
                if (_gunObject == null)
                {
                    _gunObject = FindObjectOfType<GunShooting>();
                }
                return _gunObject;
            }
        }

        public void TryShoot()
        {
            if (GunObject != null) 
                GunObject.Shoot();
        }
    }
}