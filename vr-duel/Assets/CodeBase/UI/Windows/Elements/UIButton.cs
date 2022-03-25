using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Elements
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        
        private Action _buttonCallback;

        public void Construct(Action buttonCallback)
        {
            _buttonCallback = buttonCallback;
        }

        public void SubscribeUpdates()
        {
            Button.onClick.AddListener(()=>_buttonCallback());
        }

        public void Cleanup()
        {
            Button.onClick.RemoveListener(()=>_buttonCallback());
        }
    }
}