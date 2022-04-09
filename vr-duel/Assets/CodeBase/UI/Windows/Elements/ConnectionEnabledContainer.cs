using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class ConnectionEnabledContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _activeGameObjects;
        
        public void Show()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (var activeObject in _activeGameObjects)
            {
                activeObject.SetActive(false);
            }
        }
    }
}