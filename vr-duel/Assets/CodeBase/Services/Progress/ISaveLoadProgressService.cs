using CodeBase.Entities;

namespace CodeBase.Services.Progress
{
    public interface ISaveLoadProgressService : IService
    {
        PlayerProgress LoadProgressForPlayer(string username);
        void SaveProgressForPlayer(string username, PlayerProgress progress);
    }
}