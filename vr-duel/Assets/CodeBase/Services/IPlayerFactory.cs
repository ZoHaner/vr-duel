using UnityEngine;

namespace CodeBase.Services
{
    public interface IPlayerFactory : IService
    {
        GameObject SpawnLocalPlayer();
    }
}