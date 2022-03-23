using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int WinsCount { get; private set; }

        public PlayerProgress(int winsCount)
        {
            WinsCount = winsCount;
        }

        public void AddWin()
        {
            WinsCount++;
        }
    }
}