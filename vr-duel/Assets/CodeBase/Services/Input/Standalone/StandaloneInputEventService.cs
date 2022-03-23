using System;
using CodeBase.Infrastructure;
using CodeBase.Services.Singletons;

namespace CodeBase.Services.Input.Standalone
{
    public class StandaloneInputEventService : IInputEventService
    {
        public Action AttackButtonPressed { get; set; }

        public bool InputChanged
        {
            get => _inputChanged;
            set => _inputChanged = value;
        }
        
        private readonly IUpdateProvider _updateProvider;

        private bool _attackButtonPressed;
        private bool _inputChanged;

        public StandaloneInputEventService(IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;

            Initialize();
        }

        private void Initialize()
        {
            _updateProvider.AddListener(Update);
        }

        private void Update()
        {
            var attackButtonPressed = IsAttackButtonPressed();

            _inputChanged = attackButtonPressed != _attackButtonPressed;

            if (attackButtonPressed)
            {
                AttackButtonPressed?.Invoke();
            }

            _attackButtonPressed = attackButtonPressed;
        }

        public bool IsAttackButtonPressed()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }
    }
}