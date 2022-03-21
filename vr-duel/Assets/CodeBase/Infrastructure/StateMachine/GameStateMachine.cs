﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Logic;
using CodeBase.Network;
using CodeBase.Services;
using CodeBase.Services.Network;
using CodeBase.Services.Progress;
using CodeBase.Services.ServiceLocator;
using CodeBase.Services.UpdateProvider;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using Nakama;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, InitialPointHolder pointHolder, LoadingCurtain curtain, AllServices allServices, MainThreadDispatcher dispatcher, UpdateProvider updateProvider, UnityWebRequestAdapter adapter)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, allServices, adapter, dispatcher, updateProvider),
                [typeof(ChoosingNameState)] = new ChoosingNameState(this, allServices.Single<INameSelectorService>(), allServices.Single<IPlayerDataService>(), allServices.Single<IUIFactory>(), allServices.Single<IWindowService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, allServices.Single<IProgressService>(), allServices.Single<ISaveLoadService>(), allServices.Single<IPlayerDataService>()),
                [typeof(LoadLobbyLevelState)] = new LoadLobbyLevelState(this, sceneLoader, allServices.Single<IUIFactory>(), allServices.Single<IWindowService>()),
                [typeof(LobbyCycleState)] = new LobbyCycleState(this, allServices.Single<INetworkService>(), allServices.Single<IUIFactory>(), allServices.Single<IRoundService>()),
                [typeof(LoadGameLevelState)] = new LoadGameLevelState(this, sceneLoader, curtain),
                [typeof(GameLoopState)] = new GameLoopState(allServices.Single<IRoundService>(), allServices.Single<ISaveLoadService>(), allServices.Single<IPlayerDataService>(), allServices.Single<IProgressService>()),
                [typeof(CleanupState)] = new CleanupState(allServices.Single<INetworkService>(), allServices.Single<IRoundService>())
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