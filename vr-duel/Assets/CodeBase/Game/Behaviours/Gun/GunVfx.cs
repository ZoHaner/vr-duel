using UnityEngine;

namespace CodeBase.Behaviours.Gun
{
    public class GunVfx : MonoBehaviour
    {
        public GameObject ShootVfxPrefab;

        public void PlayShootEffect(Vector3 position)
        {
            Instantiate(ShootVfxPrefab, position, Quaternion.identity);
        }
    }
}