using System;

namespace Infrastructure.Models.UI.Windows
{
    public class AuthenticationWindowModel : BaseWindowModel
    {
        public Action OnSignInAction;
        
        public void OnSignInButtonClicked() =>
            OnSignInAction?.Invoke();
    }
}