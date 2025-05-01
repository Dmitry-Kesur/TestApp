using Infrastructure.Models.UI.Windows;
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
            _authenticationWindowModel = new AuthenticationWindowModel
            {
                OnSignInAction = OnSignIn
            };
        }

        protected override BaseWindowModel GetModel() =>
            _authenticationWindowModel;

        private void OnSignIn() =>
            _authenticationService.SignIn();
    }
}