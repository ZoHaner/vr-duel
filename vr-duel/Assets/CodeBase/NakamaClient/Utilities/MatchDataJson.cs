using System.Collections.Generic;
using System.Globalization;
using CodeBase.NakamaClient.Extensions;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.NakamaClient.Utilities
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
                { $"{prefix}.velocity.x", velocity.x.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.velocity.y", velocity.y.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.velocity.z", velocity.z.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.position.x", position.x.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.position.y", position.y.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.position.z", position.z.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.rotation.x", rotation.x.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.rotation.y", rotation.y.ToString("F1", new CultureInfo("en-US").NumberFormat) },
                { $"{prefix}.rotation.z", rotation.z.ToString("F1", new CultureInfo("en-US").NumberFormat) },
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
        /// <param name="userId"></param>
        /// <param name="position">The position on death.</param>
        /// <returns>A JSONified string containing the player's position on death.</returns>
        public static string Died(string userId)
        {
            var values = new Dictionary<string, string>()
            {
                {"deadPlayerId", userId}
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