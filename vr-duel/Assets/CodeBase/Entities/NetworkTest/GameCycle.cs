using System.Threading.Tasks;

namespace CodeBase.Entities.Network
{
    public class GameCycle
    {
        private Match match;

        void StartCycle()
        {
            Cycle();
        }

        void StopCycle()
        {
            match.StopMatch();
        }

        async Task Cycle()
        {
            match = new Match();
            match.CreatePlayers();
            await RunCountdown();
            match.StartMatch();
            while (!MatchShouldEnd()) // can we wait for event ?
            {
                await Task.Delay(100);
            }

            await match.AnnounceWinner();
            match.DestroyPlayers();
        }

        private bool MatchShouldEnd()
        {
            return false; // players count <= 1 
        }

        public void OnPlayersConnected()
        {
        }

        public void OnPlayerDisconnected()
        {
        }

        public void OnPlayerCountChanged()
        {
        }

        async Task RunCountdown()
        {
        }
    }
}