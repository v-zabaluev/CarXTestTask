using Gameplay.Monsters.Spawn;
using Gameplay.Towers.Cannon;
using UI.UICannonModeChoice;
using UI.UIMonsterController;
using UnityEngine;

namespace UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private UIMonsterControlView _monsterControlView;
        [SerializeField] private Spawner _spawner;

        [SerializeField] private UICannonModeChoiceView _cannonModeChoiceView;
        [SerializeField] private CannonTower _tower;
        
        private UIMonsterControlPresenter _monsterPresenter;
        private UICannonModeChoicePresenter _cannonModeChoicePresenter;

        private void Awake()
        {
            _monsterPresenter = new UIMonsterControlPresenter(_monsterControlView, _spawner);
            _cannonModeChoicePresenter = new UICannonModeChoicePresenter(_tower, _cannonModeChoiceView);
        }
    }
}