using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services.UpdateProvider
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        public static UpdateProvider Instance
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                
                var gameObject = new GameObject(nameof(UpdateProvider));
                _instance = gameObject.AddComponent<UpdateProvider>();
                DontDestroyOnLoad(gameObject);

                return _instance;
            }
        }

        private static UpdateProvider _instance;
        private List<Action> _invokeList = new List<Action>();
        
        public void AddListener(Action action)
        {
            _invokeList.Add(action);
        }

        public void RemoveListener(Action action)
        {
            if (_invokeList.Contains(action))
            {
                _invokeList.Remove(action);
            }
        }

        private void Update()
        {
            foreach (var action in _invokeList)
            {
                action?.Invoke();
            }
        }
    }
}