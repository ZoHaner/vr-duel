using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(string address) where T : class;
        Task<T> Load<T>(AssetReferenceGameObject reference) where T : class;
    }
}