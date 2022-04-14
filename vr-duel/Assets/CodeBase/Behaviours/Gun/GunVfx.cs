using UnityEngine;

namespace CodeBase.Behaviours.Gun
{
    public class GunVfx : MonoBehaviour
    {
        [SerializeField] private GameObject ShootVfxPrefab;
        [SerializeField] private Transform _vfxParent;

        public void PlayShootEffect(Vector3 position)
        {
            Instantiate(ShootVfxPrefab, position, Quaternion.identity, _vfxParent);
        }
    }
}