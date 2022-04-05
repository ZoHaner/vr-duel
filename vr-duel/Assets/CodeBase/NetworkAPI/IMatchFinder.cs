using System.Threading.Tasks;

namespace CodeBase.NetworkAPI
{
    public interface IMatchFinder
    {
        Task StartMatchFinding();
        Task CancelMatchFinding();
    }
}