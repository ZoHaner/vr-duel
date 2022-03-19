using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class ClearPrefs
    {
        [MenuItem("Edit/Reset Playerprefs")]
        public static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}