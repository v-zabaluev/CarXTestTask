namespace UI
{
    public abstract class UIBasePresenter
    {
        protected readonly UIBaseView _view;

        protected UIBasePresenter(UIBaseView view)
        {
            _view = view;
        }

        public virtual void ShowScreen()
        {
            _view.Show();
        }

        public virtual void HideScreen()
        {
            _view.Hide();
        }
    }
}