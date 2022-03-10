using System;

namespace CodeBase.Services.Progress
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