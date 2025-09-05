using Gameplay.Towers.Cannon;

namespace UI.UICannonModeChoice
{
    public class UICannonModeChoicePresenter : UIBasePresenter
    {
        private readonly CannonTower _tower;
        private readonly UICannonModeChoiceView _view;

        public UICannonModeChoicePresenter(CannonTower tower, UICannonModeChoiceView view) : base(view)
        {
            _tower = tower;
            _view = view;

            _view.CannonModeButton.onClick.AddListener(() => _tower.SetCannonMode(CannonType.Cannon));
            _view.MortarModeButton.onClick.AddListener(() => _tower.SetCannonMode(CannonType.Mortar));
        }
    }
}