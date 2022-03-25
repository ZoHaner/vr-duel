using System;
using UnityEngine;

namespace CodeBase.Services.Input.TestMonoService
{
    public class MenuButtonPressed : InputEventBase
    {
        public MenuButtonPressed(Action callback) : base(callback)
        {
        }

        public override bool Condition()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Y);
        }
    }
}