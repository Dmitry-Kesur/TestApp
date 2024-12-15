using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public abstract class BaseWindowController<T> : BaseWindowController where T : BaseWindow
    {
        protected T windowView;
        
        public override void OnWindowAdd(BaseWindow view)
        {
            windowView = (T)view;
        }
    }
    
    public abstract class BaseWindowController
    {
        public abstract void OnWindowAdd(BaseWindow view);
    }
}