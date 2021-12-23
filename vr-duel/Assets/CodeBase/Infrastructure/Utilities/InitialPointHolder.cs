using UnityEngine;

namespace CodeBase.Infrastructure.Utilities
{
    public class InitialPointHolder
    {
        private readonly float _distance = 10f;
        private readonly Vector3 _basePoint = Vector3.zero;

        public Vector3 GetInitialPoint(int playerIndex)
        {
            float zShift =  playerIndex * _distance;
            return new Vector3(_basePoint.x, _basePoint.y, _basePoint.z + zShift);
        }
    }
}