using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.Singletons
{
    public class UpdateProvider : MonoBehaviour, IUpdateProvider
    {
        public static UpdateProvider Instance => GetOrCreateInstance();
        private static UpdateProvider _instance;

        private readonly List<Action> _invokeList = new List<Action>();

        public void AddListener(Action action) => 
            _invokeList.Add(action);

        public void RemoveListener(Action action)
        {
            if (_invokeList.Contains(action))
            {
                _invokeList.Remove(action);
            }
        }

        private void Update()
        {
            foreach (var listeners in _invokeList)
            {
                listeners?.Invoke();
            }
        }

        private static UpdateProvider GetOrCreateInstance()
        {
            return _instance != null ? 
                _instance : 
                CreateInstance();
        }

        private static UpdateProvider CreateInstance()
        {
            var gameObject = new GameObject(nameof(UpdateProvider));
            _instance = gameObject.AddComponent<UpdateProvider>();
            DontDestroyOnLoad(gameObject);
            return _instance;
        }
    }
}