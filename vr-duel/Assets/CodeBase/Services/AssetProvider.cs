using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services
{
    public class AssetProvider : IAssetProvider
    {
        public async Task<T> Load<T>(string address) where T : class
        {
            return await Addressables.LoadAssetAsync<T>(address).Task;
        }
    }
}