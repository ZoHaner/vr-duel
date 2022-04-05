using System.Threading.Tasks;

namespace CodeBase.NetworkAPI
{
    public interface IMatchFinder
    {
        Task StartMatchFindingAsync();
        Task CancelMatchFindingAsync();
    }
}