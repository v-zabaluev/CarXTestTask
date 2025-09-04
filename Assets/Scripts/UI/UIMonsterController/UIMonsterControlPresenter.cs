using Gameplay.Monsters;
using Gameplay.Monsters.Spawn;

namespace UI.UIMonsterController
{
    public class UIMonsterControlPresenter : UIBasePresenter
    {
        private readonly UIMonsterControlView _view;
        private readonly Spawner _spawner;

        public UIMonsterControlPresenter(UIMonsterControlView view, Spawner spawner) : base(view)
        {
            _view = view;
            _spawner = spawner;

            _view.LinearButton.onClick.AddListener(() => SwitchMovement(MonsterMovementType.Linear));
            _view.AcceleratedButton.onClick.AddListener(() => SwitchMovement(MonsterMovementType.Accelerated));
            _view.CircularButton.onClick.AddListener(() => SwitchMovement(MonsterMovementType.Circular));
            _view.PathFollowButton.onClick.AddListener(() => SwitchMovement(MonsterMovementType.PathFollow));
        }

        private void SwitchMovement(MonsterMovementType type)
        {
            _spawner.SwitchMovement(type);
        }
    }
}