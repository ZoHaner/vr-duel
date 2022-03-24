using System;

namespace CodeBase.Entities
{
    [Serializable]
    public class PlayerProgress
    {
        public Action<int> Changed;
        public int WinsCount { get; private set; }

        public PlayerProgress(int winsCount)
        {
            WinsCount = winsCount;
        }

        public void AddWin()
        {
            WinsCount++;
            Changed?.Invoke(WinsCount);
        }
    }
}