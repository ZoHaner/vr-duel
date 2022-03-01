using System;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class WindowBase : MonoBehaviour
    {
        private void Awake() => 
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            Cleanup();


        protected virtual void OnAwake() {}
        protected virtual void Initialize() {}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}