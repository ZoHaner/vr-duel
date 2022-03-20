using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class NameListContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _selectNameButtonPrefab;
        [SerializeField] private Transform _listParent;
        private INameSelectorService _nameSelectorService;
        
        public void Construct(INameSelectorService nameSelectorService)
        {
            _nameSelectorService = nameSelectorService;
        }

        public void Initialize()
        {
            foreach (var playerName in _nameSelectorService.GetSavedPlayersNames())
            {
                var item = Instantiate(_selectNameButtonPrefab, _listParent).GetComponent<PlayerNameItem>();
                item.Construct(_nameSelectorService);
                item.Initialize(playerName);
            }
        }
    }
}