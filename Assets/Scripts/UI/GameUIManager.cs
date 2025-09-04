using Gameplay.Monsters.Spawn;
using UI.UIMonsterController;
using UnityEngine;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private UIMonsterControlView _monsterControlView;
        [SerializeField] private Spawner _spawner;

        private UIMonsterControlPresenter _monsterPresenter;

        private void Awake()
        {
            _monsterPresenter = new UIMonsterControlPresenter(_monsterControlView, _spawner);
        }
    }
}