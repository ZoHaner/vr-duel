namespace CodeBase.Services.Progress
{
    public interface ISaveLoadService : IService
    {
        PlayerProgress LoadProgressForPlayer(string username);
        void SaveProgressForPlayer(string username, PlayerProgress progress);
    }
}