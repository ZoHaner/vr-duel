using System.Collections.Generic;
using CodeBase.Extensions;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.Player
{
    /// <summary>
    /// A static class that creates JSON string network messages.
    /// </summary>
    public static class MatchDataJson
    {
        /// <summary>
        /// Creates a network message containing velocity and position.
        /// </summary>
        /// <param name="headVelocity">The velocity to send.</param>
        /// <param name="headPosition">The position to send.</param>
        /// <returns>A JSONified string containing velocity and position data.</returns>
        public static string BodyVelocityAndPosition(
            Vector3 headVelocity, 
            Transform headTransform, 
            Vector3 lhandVelocity, 
            Transform lhandTransform,
            Vector3 rhandVelocity, 
            Transform rhandTransform)
        {
            var values = new Dictionary<string, string>();
            values.AddRange(VelocityAndPosition("head", headVelocity, headTransform.position, headTransform.eulerAngles));
            values.AddRange(VelocityAndPosition("lhand", lhandVelocity, lhandTransform.position, lhandTransform.eulerAngles));
            values.AddRange(VelocityAndPosition("rhand", rhandVelocity, rhandTransform.position, rhandTransform.eulerAngles));

            return values.ToJson();
        }

        private static Dictionary<string, string> VelocityAndPosition(string prefix, Vector3 velocity, Vector3 position, Vector3 rotation)
        {
            return new Dictionary<string, string>
            {
                { $"{prefix}.velocity.x", velocity.x.ToString() },
                { $"{prefix}.velocity.y", velocity.y.ToString() },
                { $"{prefix}.velocity.z", velocity.z.ToString() },
                { $"{prefix}.position.x", position.x.ToString() },
                { $"{prefix}.position.y", position.y.ToString() },
                { $"{prefix}.position.z", position.z.ToString() },
                { $"{prefix}.rotation.x", rotation.x.ToString() },
                { $"{prefix}.rotation.y", rotation.y.ToString() },
                { $"{prefix}.rotation.z", rotation.z.ToString() },
            };
        }

        /// <summary>
        /// Creates a network message containing player input.
        /// </summary>
        /// <param name="horizontalInput">The current horizontal input.</param>
        /// <param name="jump">The jump input.</param>
        /// <param name="jumpHeld">The jump held input.</param>
        /// <param name="attack">The attack input.</param>
        /// <returns>A JSONified string containing player input.</returns>
        public static string Input(bool attack)
        {
            var values = new Dictionary<string, string>
            {
                { "attack", attack.ToString() }
            };

            return values.ToJson();
        }

        /// <summary>
        /// Creates a network message specifying that the player died and the position when they died.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="position">The position on death.</param>
        /// <returns>A JSONified string containing the player's position on death.</returns>
        public static string Died(string sessionId)
        {
            var values = new Dictionary<string, string>()
            {
                {"deadPlayerSessionId", sessionId}
            };
            return values.ToJson();
        }

        /// <summary>
        /// Creates a network message specifying that the player respawned and at what spawn point.
        /// </summary>
        /// <param name="spawnIndex">The spawn point.</param>
        /// <returns>A JSONified string containing the player's respawn point.</returns>
        public static string Respawned(int spawnIndex)
        {
            var values = new Dictionary<string, string>
            {
                { "spawnIndex", spawnIndex.ToString() },
            };

            return values.ToJson();
        }

        /// <summary>
        /// Creates a network message indicating a new round should begin and who won the previous round.
        /// </summary>
        /// <param name="winnerPlayerName">The winning player's name.</param>
        /// <returns>A JSONified string containing the winning players name.</returns>
        public static string StartNewRound(string winnerPlayerName)
        {
            var values = new Dictionary<string, string>
            {
                { "winningPlayerName", winnerPlayerName }
            };

            return values.ToJson();
        }
    }
}