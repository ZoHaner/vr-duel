using UnityEngine;

namespace CodeBase.Utilities
{
    public class SetResolutionMode : MonoBehaviour
    {
        void Awake()
        {
            Screen.SetResolution(1024, 600, FullScreenMode.Windowed);
        }
    }
}
