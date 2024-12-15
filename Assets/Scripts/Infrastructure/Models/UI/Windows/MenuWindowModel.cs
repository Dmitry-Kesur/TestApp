﻿using System;
using Infrastructure.Services;

namespace Infrastructure.Models.UI.Windows
{
    public class MenuWindowModel : BaseWindowModel
    {
        private readonly IPlayerProgressService _playerProgressService;
        private readonly IAuthenticationService _authenticationService;

        public Action OnPlayButtonClickAction;
        public Action OnSettingsButtonClickAction;
        public Action OnShopButtonClickAction;
        public Action OnBoostersButtonClickAction;

        public MenuWindowModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public int BestScore { get; set; }

        public void PlayButtonClick() =>
            OnPlayButtonClickAction?.Invoke();

        public void OnSettingsButtonClick() =>
            OnSettingsButtonClickAction?.Invoke();

        public void OnShopButtonClick() =>
            OnShopButtonClickAction?.Invoke();

        public void OnBoostersButtonClick() =>
            OnBoostersButtonClickAction?.Invoke();
    }
}