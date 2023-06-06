using UnityEngine;

namespace UI
{
    public class BaseView : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnHide()
        {
         
        }

        protected virtual void OnShow()
        {
            
        }
    }
}