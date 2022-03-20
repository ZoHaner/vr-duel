namespace CodeBase.Services
{
    public interface IStorageService : IService
    {
        string ReadData(string key);
        void WriteData(string key, string data);
    }
}