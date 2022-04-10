using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CodeBase.Behaviours.Gun;
using CodeBase.Behaviours.Player.Remote.Lerp;
using CodeBase.Entities;
using CodeBase.StaticData;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote
{
    public class RemotePlayerSync : MonoBehaviour
    {
        public float LerpTime = 0.05f;

        public Transform HeadTransform;
        public Transform LeftHandTransform;
        public Transform RightHandTransform;

        public GunShooting Gun;

        private readonly Dictionary<string, PositionSync> _positionsToSync = new Dictionary<string, PositionSync>();
        private readonly Dictionary<string, RotationSync> _rotationsToSync = new Dictionary<string, RotationSync>();

        private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");
        private RemotePlayer _networkData;

        public void Construct(RemotePlayer networkData, GunShooting gun)
        {
            _networkData = networkData;
            Gun = gun;

            FillPositionSyncList();
            FillRotationSyncList();
        }

        private void LateUpdate()
        {
            LerpPosition();
            LerpRotation();
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
            return matchState.UserPresence.SessionId != _networkData.Presence.SessionId;
        }

        private void UpdateVelocityAndPositionFromState(byte[] state)
        {
            IDictionary<string, string> stateDictionary = GetStateAsDictionary(state);

            SetTargetPositions(stateDictionary);
            SetTargetRotations(stateDictionary);
        }

        private void SetTargetRotations(IDictionary<string, string> stateDictionary)
        {
            foreach (var prefix in _rotationsToSync.Keys)
            {
                var newRotation = GetRotationFromDictionary(stateDictionary, prefix);
                _rotationsToSync[prefix].SetNewTarget(newRotation);
            }
        }

        private void SetTargetPositions(IDictionary<string, string> stateDictionary)
        {
            foreach (var prefix in _positionsToSync.Keys)
            {
                var newPosition = GetPositionFromDictionary(stateDictionary, prefix);
                _positionsToSync[prefix].SetNewTarget(newPosition);
            }
        }

        private void LerpPosition()
        {
            foreach (var positionSync in _positionsToSync.Values)
                positionSync.ApplyLerp(Time.deltaTime);
        }

        private void LerpRotation()
        {
            foreach (var rotationSync in _rotationsToSync.Values)
                rotationSync.ApplyLerp(Time.deltaTime);
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

        private void FillPositionSyncList()
        {
            _positionsToSync["head"] = new PositionSync(HeadTransform, new VectorLerp(LerpTime));
            _positionsToSync["lhand"] = new PositionSync(LeftHandTransform, new VectorLerp(LerpTime));
            _positionsToSync["rhand"] = new PositionSync(RightHandTransform, new VectorLerp(LerpTime));
        }

        private void FillRotationSyncList()
        {
            _rotationsToSync["head"] = new RotationSync(HeadTransform, new QuaternionLerp(LerpTime));
            _rotationsToSync["lhand"] = new RotationSync(LeftHandTransform, new QuaternionLerp(LerpTime));
            _rotationsToSync["rhand"] = new RotationSync(RightHandTransform, new QuaternionLerp(LerpTime));
        }

        private Vector3 GetPositionFromDictionary(IDictionary<string, string> dictionary, string prefix)
        {
            return new Vector3(
                ParseFloat(dictionary[$"{prefix}.position.x"]),
                ParseFloat(dictionary[$"{prefix}.position.y"]),
                ParseFloat(dictionary[$"{prefix}.position.z"])
            );
        }

        private Quaternion GetRotationFromDictionary(IDictionary<string, string> dictionary, string prefix)
        {
            return new Quaternion(
                ParseFloat(dictionary[$"{prefix}.rotation.x"]),
                ParseFloat(dictionary[$"{prefix}.rotation.y"]),
                ParseFloat(dictionary[$"{prefix}.rotation.z"]),
                ParseFloat(dictionary[$"{prefix}.rotation.w"])
            );
        }

        private float ParseFloat(string number)
        {
            return float.Parse(number, _cultureInfo);
        }

        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }
    }
}