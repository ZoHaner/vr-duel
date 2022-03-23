using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services;
using Nakama;
using UnityEngine;

namespace CodeBase.UI.Windows.Elements
{
    public class MatchListContainer : MonoBehaviour
    {
        public GameObject[] EmptyListItems;
        public Transform Parent;
        public MatchItem ItemPrefab;
        
        private INetworkService _networkService;
        private Coroutine _matchSearchingCoroutine;

        private IApiMatchList _matches;

        private const float MatchSearchDelay = 1f;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public void Initialize()
        {
            RefreshNonAvailableItems();
            RunMatchSearching();
        }

        public void SubscribeUpdates()
        {
            _networkService.MatchListFound += UpdateList;
        }


        public void Cleanup()
        {
            _networkService.MatchListFound -= UpdateList;
            StopMatchSearching();
        }

        private void RefreshNonAvailableItems()
        {
            bool activity = !IsListExist();
            
            foreach (var item in EmptyListItems)
            {
                item.SetActive(activity);
            }
        }

        private void UpdateList(IApiMatchList matches)
        {
            _matches = matches;
            
            RefreshNonAvailableItems();
            DestroyList();
            CreateList(matches.Matches);
        }

        private void DestroyList()
        {
            if(Parent.childCount == 0)
                return;
            
            for (int i = Parent.childCount -1; i < 0; i--)
            {
                Destroy(Parent.GetChild(i));
            }
        }

        private void CreateList(IEnumerable<IApiMatch> matches)
        {
            foreach (IApiMatch match in matches) 
                CreateMatchItem(match);
        }

        private void CreateMatchItem(IApiMatch match)
        {
            var item = Instantiate(ItemPrefab, Parent).GetComponent<MatchItem>();
            item.Construct(_networkService, match);
        }

        private bool IsListExist()
        {
            if (_matches == null)
                return false;
            
            return _matches.Matches.Any();
        }

        private void RunMatchSearching()
        {
            _matchSearchingCoroutine = StartCoroutine(MatchSearching());
        }

        private void StopMatchSearching()
        {
            if(_matchSearchingCoroutine != null)
                StopCoroutine(_matchSearchingCoroutine);
        }

        private IEnumerator MatchSearching()
        {
            while (true)
            {
                _networkService.GetMatchList();
                yield return new WaitForSeconds(MatchSearchDelay);
            }
        }
    }
}