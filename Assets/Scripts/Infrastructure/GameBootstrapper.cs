using Infrastructure.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, Instantiate(_loadingCurtainPrefab));

            _game.StateMachine.Enter<BootStrapState>();

            DontDestroyOnLoad(this);
        }
    }
}