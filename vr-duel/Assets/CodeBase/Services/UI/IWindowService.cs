namespace CodeBase.Services.UI
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}