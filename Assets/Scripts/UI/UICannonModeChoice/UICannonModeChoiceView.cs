using UnityEngine;
using UnityEngine.UI;

namespace UI.UICannonModeChoice
{
    public class UICannonModeChoiceView : UIBaseView
    {
        [SerializeField] private Button _cannonModeButton;
        [SerializeField] private Button _mortarModeButton;
        
        public Button CannonModeButton => _cannonModeButton;
        public Button MortarModeButton => _mortarModeButton;
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}