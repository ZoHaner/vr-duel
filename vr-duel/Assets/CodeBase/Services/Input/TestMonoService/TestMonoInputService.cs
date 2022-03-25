using System;
using UnityEngine;

namespace CodeBase.Services.Input.TestMonoService
{
    public class TestMonoInputService : MonoBehaviour
    {
        private Action MenuButtonPressedEvent;

        private InputEventBase[] _inputs;

        void Initialize()
        {
            _inputs = new InputEventBase[]
            {
                new MenuButtonPressed(MenuButtonPressedEvent)
            };
        }

        private void Update()
        {
            CheckConditionsAndExecuteCallbacks();
        }

        private void CheckConditionsAndExecuteCallbacks()
        {
            foreach (InputEventBase input in _inputs)
            {
                if (input.CheckCondition())
                    input.InvokeCallback();
            }
        }
    }
}