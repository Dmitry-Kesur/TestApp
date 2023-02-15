using UnityEngine;

namespace UI
{
    public class BaseView : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            AfterShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            AfterHide();
        }

        protected virtual void AfterHide()
        {
         
        }

        protected virtual void AfterShow()
        {
            
        }
    }
}