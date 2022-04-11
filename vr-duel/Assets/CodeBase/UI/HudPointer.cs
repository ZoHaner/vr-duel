using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class HudPointer : MonoBehaviour
    {
        [SerializeField] private Image _pointer;

        public void Show()
        {
            _pointer.enabled = true;
        }

        public void Hide()
        {
            _pointer.enabled = false;
        }
    }
}