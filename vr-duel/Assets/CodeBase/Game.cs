using CodeBase.Infrastructure;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.ServiceLocator;
using CodeBase.Services.Singletons;
using CodeBase.UI;
using CodeBase.Utilities;

namespace CodeBase
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner,
            LoadingCurtain loadingCurtain,
            MainThreadDispatcher mainThreadDispatcher,
            UpdateProvider updateProvider,
            AllServices allServices)
        {
            StateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                loadingCurtain,
                allServices,
                mainThreadDispatcher, 
                updateProvider);
        }
    }
}