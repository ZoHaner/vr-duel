namespace CodeBase.ServiceLocator
{
    public class AllServices
    {
        public static AllServices Container => _instance ??= new AllServices();
        private static AllServices _instance;

        public void Register<T>(T implementation) where T : IService => 
            Implementation<T>.ServiceInstance = implementation;

        public T Single<T>() where T : IService => 
            Implementation<T>.ServiceInstance;

        private class Implementation<T> where T : IService
        {
            public static T ServiceInstance;
        }
    }
}