using System.Threading.Tasks;

namespace CodeBase.Services
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(string address) where T : class;
    }
}