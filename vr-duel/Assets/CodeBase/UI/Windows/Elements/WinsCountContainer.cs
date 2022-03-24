using CodeBase.Services.Progress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class WinsCountContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI WinsTextField;
        
        private IProgressService _progressService;

        public void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public void Initialize()
        {
            WinsTextField.text = $"{_progressService.Progress.WinsCount} wins total";
        }
    }
}