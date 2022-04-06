using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class StandalonePlayerMovement : PlayerMovement
    {
        protected override void Move()
        {
            Vector3 vector = CalculateMovingVector();
            MovePlayer(vector);
        }
    }
}