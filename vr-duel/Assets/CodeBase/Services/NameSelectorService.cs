using System;
using CodeBase.Infrastructure.StateMachine.States;

namespace CodeBase.Services
{
    public class NameSelectorService : INameSelectorService
    {
        public Action<string> OnSelect
        {
            get => _onSelect;
            set
            {
                _onSelect = value;
                SendUsername();
            }
        }

        private Action<string> _onSelect;

        private void SendUsername()
        {
            string username;

#if UNITY_EDITOR
            username = "Editor";
#else
            username = "Build";
#endif

            OnSelect?.Invoke(username);
        }
    }
}