using Infrastructure.SceneTransition;
using Services;

namespace Infrastructure.States
{
    public class BootStrapState : IState
    {
        private const string InitialSceneName = "Preload";

        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;

        public BootStrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
            Register();
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>("Start");
        }

        private void Register()
        {
            StaticDataService.LoadAll();
        }

        public void Exit()
        {
        }
    }
}