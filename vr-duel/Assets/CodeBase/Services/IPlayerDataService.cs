using CodeBase.Data;

namespace CodeBase.Services
{
    public interface IPlayerDataService : IService
    {
        UserAccount User { get; set; }
    }
}