using System.Collections.Generic;
using System.Text;
using CodeBase.Behaviours.Gun;
using CodeBase.Entities;
using CodeBase.StaticData;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote
{
    public class RemotePlayerSync : MonoBehaviour
    {
        public RemotePlayer NetworkData;
        
        public float LerpTime = 0.05f;

        public Transform HeadTransform;
        public Transform LeftHandTransform;
        public Transform RightHandTransform;

        public Rigidbody HeadRigidbody;
        public Rigidbody LeftHandRigidbody;
        public Rigidbody RightHandRigidbody;

        public GunShooting Gun;
        
        private Rigidbody2D playerRigidbody;
        private Transform playerTransform;

        public void Construct(RemotePlayer networkData, GunShooting gun)
        {
            NetworkData = networkData;
            Gun = gun;
        }

        public void UpdateState(IMatchState matchState)
        {
            if (NotMyMatchState(matchState))
                return;

            switch (matchState.OpCode)
            {
                case OpCodes.VelocityAndPosition:
                    UpdateVelocityAndPositionFromState(matchState.State);
                    break;
                case OpCodes.Input:
                    UpdateInput(matchState.State);
                    break;
            }
        }

        private bool NotMyMatchState(IMatchState matchState)
        {
            return matchState.UserPresence.SessionId != NetworkData.Presence.SessionId;
        }

        private void UpdateVelocityAndPositionFromState(byte[] state)
        {
            IDictionary<string, string> stateDictionary = GetStateAsDictionary(state);
            
            HeadTransform.position = GetPositionFromDictionary(stateDictionary, "head", "position");
            LeftHandTransform.position = GetPositionFromDictionary(stateDictionary, "lhand", "position");
            RightHandTransform.position = GetPositionFromDictionary(stateDictionary, "rhand", "position");
            
            HeadTransform.rotation = Quaternion.Euler(GetPositionFromDictionary(stateDictionary, "head", "rotation"));
            LeftHandTransform.rotation = Quaternion.Euler(GetPositionFromDictionary(stateDictionary, "lhand", "rotation"));
            RightHandTransform.rotation = Quaternion.Euler(GetPositionFromDictionary(stateDictionary, "rhand", "rotation"));
            
        }

        private void UpdateInput(byte[] state)
        {
            IDictionary<string, string> stateDictionary = GetStateAsDictionary(state);

            var attack = bool.Parse(stateDictionary["attack"]);
            if (attack)
            {
                Gun.Shoot();
            }

        }

        private Vector3 GetPositionFromDictionary(IDictionary<string, string> dictionary, string prefix, string attribute)
        {
            return new Vector3(
                float.Parse(dictionary[$"{prefix}.{attribute}.x"]),
                float.Parse(dictionary[$"{prefix}.{attribute}.y"]),
                float.Parse(dictionary[$"{prefix}.{attribute}.z"])
            );
        }


        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }
    }
}