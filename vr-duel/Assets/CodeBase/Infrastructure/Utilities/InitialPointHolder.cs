using UnityEngine;

namespace CodeBase.Infrastructure.Utilities
{
    public class InitialPointHolder
    {
        private GameObject[] SpawnPoints;

        public Vector3 GetInitialPoint()
        {
            if (SpawnPoints == null)
                SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

            return SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform.position;
        }
    }
}