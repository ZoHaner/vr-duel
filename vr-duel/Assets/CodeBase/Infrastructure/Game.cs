using CodeBase.Behaviours;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Services.ServiceLocator;
using CodeBase.Services.Singletons;
using Nakama;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner,
            LoadingCurtain loadingCurtain,
            MainThreadDispatcher mainThreadDispatcher,
            UnityWebRequestAdapter unityWebRequestAdapter,
            UpdateProvider updateProvider,
            AllServices allServices)
        {
            StateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                loadingCurtain,
                allServices,
                mainThreadDispatcher, 
                updateProvider,
                unityWebRequestAdapter);
        }
    }
}