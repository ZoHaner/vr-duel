using CodeBase.Services.Network;
using CodeBase.UI.Windows.Elements;

namespace CodeBase.UI.Windows
{
    public class MatchListWindow : WindowBase
    {
        public MatchListContainer MatchListContainer;
        
        private INetworkService _networkService;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
            
            MatchListContainer.Construct(networkService);
        }

        protected override void Initialize()
        {
            MatchListContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            MatchListContainer.SubscribeUpdates();
        }

        protected override void Cleanup()
        {
            MatchListContainer.Cleanup();
        }
    }
}