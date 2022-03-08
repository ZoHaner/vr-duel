namespace CodeBase.Services.StaticData
{
    public class PlayerStaticData
    {
        public string Name;
        public int WinsCount;

        public void AddWinToPlayer()
        {
            WinsCount++;
        }
    }
}