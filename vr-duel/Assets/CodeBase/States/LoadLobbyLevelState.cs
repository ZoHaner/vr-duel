﻿using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.StaticData;
using CodeBase.Utilities;

namespace CodeBase.States
{
    public class LoadLobbyLevelState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IPlayerFactory _playerFactory;

        public LoadLobbyLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IPlayerFactory playerFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _playerFactory = playerFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(AssetsPath.LobbySceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            _playerFactory.SpawnMovingLocalPlayer();
            _gameStateMachine.Enter<LobbyCycleState>();
        }

        public void Exit()
        {
        }
    }
}