using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public abstract class BaseWindowController<T> : BaseWindowController where T : BaseWindow
    {
        protected T windowView;
        
        public override void SetWindowView(BaseWindow view)
        {
            windowView = (T)view;
            windowView.SetModel(GetModel());
        }

        public override void AfterWindowCreate()
        {
            InitParameters();
        }

        protected virtual void InitParameters()
        {
           
        }

        protected abstract BaseWindowModel GetModel();
    }
    
    public abstract class BaseWindowController
    {
        public abstract void SetWindowView(BaseWindow view);

        public abstract void AfterWindowCreate();
    }
}