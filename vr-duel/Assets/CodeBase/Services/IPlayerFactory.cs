using UnityEngine;

namespace CodeBase.Services
{
    public interface IPlayerFactory : IService
    {
        GameObject SpawnMovingLocalPlayer();
        GameObject SpawnStaticLocalPlayer();
    }
}