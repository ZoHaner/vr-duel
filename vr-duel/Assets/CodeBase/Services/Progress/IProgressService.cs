using CodeBase.Entities;

namespace CodeBase.Services.Progress
{
    public interface IProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}