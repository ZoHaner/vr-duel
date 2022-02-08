using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, NetworkService networkService, InitialPointHolder initialPointHolder, LoadingCurtain loadingCurtain)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLobbyLevelState)] = new LoadLobbyLevelState(this, sceneLoader),
                [typeof(LobbyCycleState)] = new LobbyCycleState(this, networkService),
                [typeof(LoadGameLevelState)] = new LoadGameLevelState(this, sceneLoader, initialPointHolder, loadingCurtain, networkService),
                [typeof(GameLoopState)] = new GameLoopState(),
                
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}