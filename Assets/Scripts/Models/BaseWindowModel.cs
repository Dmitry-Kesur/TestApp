using UI;

namespace DefaultNamespace
{
    public class BaseWindowModel
    {
        protected BaseWindow windowInstance;
        
        public virtual BaseWindow GetWindowInstance()
        {
            return null;
        }
    }
}