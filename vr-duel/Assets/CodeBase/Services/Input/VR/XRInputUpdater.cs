using System.Collections.Generic;
using CodeBase.Services.Singletons;

namespace CodeBase.Services.Input.VR
{
    public class XRInputUpdater
    {
        private readonly List<IXRInput> _bindings = new List<IXRInput>();
        private readonly IUpdateProvider _updateProvider;

        public XRInputUpdater(IUpdateProvider updateProvider)
        {
            _updateProvider = updateProvider;
        }

        public void Initialize()
        {
            _updateProvider.AddListener(UpdateBindingsValues);
        }

        public void AddBinding(IXRInput binding)
        {
            if (!_bindings.Contains(binding))
                _bindings.Add(binding);
        }

        public void RemoveBinding(IXRInput binding)
        {
            if (_bindings.Contains(binding))
                _bindings.Remove(binding);
        }

        private void UpdateBindingsValues()
        {
            foreach (var binding in _bindings)
            {
                binding.UpdateValue();
            }
        }
    }
}