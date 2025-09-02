using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine : MonoBehaviour
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootStrapState)] = new BootStrapState(this, sceneLoader),
                [typeof(LoadLevelState)] =
                    new LoadLevelState(this, sceneLoader, loadingCurtain, GameFactory.Instance),
                [typeof(GameLoopState)] = new GameLoopState(this, sceneLoader),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();

            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
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