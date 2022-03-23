using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int WinsCount;

        public PlayerProgress(int winsCount)
        {
            WinsCount = winsCount;
        }
    }
}