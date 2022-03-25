using System;

namespace CodeBase.Services.Input.TestMonoService
{
    public abstract class InputEventBase
    {
        public bool InputChangedSinceLastCheck { get; private set; }

        private readonly Action _callback;
        private bool _lastResult;

        public InputEventBase(Action callback) =>
            _callback = callback;

        public abstract bool Condition();

        public bool CheckCondition()
        {
            var currentResult = Condition();
            CheckIfResultChanges(currentResult);
            WriteCurrentResult(currentResult);
            return currentResult;
        }

        public void InvokeCallback() =>
            _callback?.Invoke();

        private void WriteCurrentResult(bool currentResult) => 
            _lastResult = currentResult;

        private void CheckIfResultChanges(bool currentResult) => 
            InputChangedSinceLastCheck = _lastResult != currentResult;
    }
}