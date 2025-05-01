using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public abstract class BaseWindowController<T> : BaseWindowController where T : BaseWindow
    {
        protected T windowView;
        
        public override void OnWindowCreate(BaseWindow view)
        {
            windowView = (T)view;
            windowView.SetModel(GetModel());
            windowView.OnCreate();
        }

        protected virtual BaseWindowModel GetModel() =>
            null;
    }
    
    public abstract class BaseWindowController
    {
        public abstract void OnWindowCreate(BaseWindow view);
    }
}