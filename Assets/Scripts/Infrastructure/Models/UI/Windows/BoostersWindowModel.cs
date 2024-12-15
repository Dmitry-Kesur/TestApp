using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Services;

namespace Infrastructure.Models.UI.Windows
{
    public class BoostersWindowModel : BaseWindowModel
    {
        private readonly IBoostersService _boostersService;
        public Action OnBackToMenuAction;

        public BoostersWindowModel(IBoostersService boostersService)
        {
            _boostersService = boostersService;
        }

        public List<BoosterModel> GetBoosters() =>
            _boostersService.Boosters;

        public void OnBackToMenuButtonClicked() =>
            OnBackToMenuAction?.Invoke();
    }
}