using System;

namespace CodeBase.Entities
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