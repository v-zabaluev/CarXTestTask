using UnityEngine;
using UnityEngine.UI;

namespace UI.UIMonsterController
{
    public class UIMonsterControlView : UIBaseView
    {
        [Header("Buttons")]
        [SerializeField] private Button _linearButton;
        [SerializeField] private Button _acceleratedButton;
        [SerializeField] private Button _circularButton;
        [SerializeField] private Button _pathFollowButton;

        public Button LinearButton => _linearButton;
        public Button AcceleratedButton => _acceleratedButton;
        public Button CircularButton => _circularButton;
        public Button PathFollowButton => _pathFollowButton;

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