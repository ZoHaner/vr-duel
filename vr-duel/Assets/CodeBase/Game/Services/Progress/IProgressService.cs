using CodeBase.Entities;
using CodeBase.ServiceLocator;

namespace CodeBase.Services.Progress
{
    public interface IProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}