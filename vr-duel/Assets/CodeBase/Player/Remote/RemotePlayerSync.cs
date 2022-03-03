using System.Collections.Generic;
using System.Text;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.Player.Remote
{
    public class RemotePlayerSync : MonoBehaviour
    {
        public RemotePlayerNetworkData NetworkData;
        
        public float LerpTime = 0.05f;

        public Transform HeadTransform;
        public Transform LeftHandTransform;
        public Transform RightHandTransform;

        public Rigidbody HeadRigidbody;
        public Rigidbody LeftHandRigidbody;
        public Rigidbody RightHandRigidbody;
        
        private Rigidbody2D playerRigidbody;
        private Transform playerTransform;

        public void Construct(RemotePlayerNetworkData networkData)
        {
            NetworkData = networkData;
        }

        public void UpdateState(IMatchState matchState)
        {
            if (matchState.UserPresence.SessionId != NetworkData.User.SessionId)
            {
                return;
            }

            switch (matchState.OpCode)
            {
                case OpCodes.VelocityAndPosition:
                    UpdateVelocityAndPositionFromState(matchState.State);
                    break;
            }
        }
        
        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
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

        private Vector3 GetPositionFromDictionary(IDictionary<string, string> dictionary, string prefix, string attribute)
        {
            return new Vector3(
                float.Parse(dictionary[$"{prefix}.{attribute}.x"]),
                float.Parse(dictionary[$"{prefix}.{attribute}.y"]),
                float.Parse(dictionary[$"{prefix}.{attribute}.z"])
            );
        }
    }
}