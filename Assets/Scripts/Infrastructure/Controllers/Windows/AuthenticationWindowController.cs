using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Authentication;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class AuthenticationWindowController : BaseWindowController<AuthenticationWindow>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthenticationWindowModel _authenticationWindowModel;

        public AuthenticationWindowController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _authenticationWindowModel = new AuthenticationWindowModel();
            _authenticationWindowModel.OnSignInAction = OnSignIn;
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            windowView.SetModel(_authenticationWindowModel);
        }

        private void OnSignIn()
        {
            _authenticationService.SignIn();
        }
    }
}