﻿using UnityEngine;

namespace CodeBase.Infrastructure.Utilities
{
    public class ResourcesUtilities
    {
        public static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public static GameObject Instantiate(string path, Vector3 at, Vector3 rotation)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.Euler(rotation));
        }
    }
}