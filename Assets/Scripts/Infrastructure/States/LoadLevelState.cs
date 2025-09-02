using Infrastructure.Factory;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private LoadingCurtain _loadingCurtain;
        private IGameFactory _gameFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        private void OnLoaded()
        {
            //Create towers and initialize it
            _stateMachine.Enter<GameLoopState>();
            
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }
    }
}