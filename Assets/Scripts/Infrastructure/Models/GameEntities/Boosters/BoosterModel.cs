using System;
using Infrastructure.Data.Boosters;
using Infrastructure.Models.GameEntities.Resources;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Boosters
{
    public class BoosterModel : IDrawableModel
    {
        private readonly BoosterData _boosterData;
        private readonly ResourceModel _boosterResource;

        public event Action<BoosterModel> ActivateBoosterAction;

        public BoosterModel(BoosterData boosterData, ResourceModel boosterResource)
        {
            _boosterData = boosterData;
            _boosterResource = boosterResource;
        }
        
        public int Id =>
            _boosterData.Id;

        public int BoostValue =>
            _boosterData.BoostValue;

        public int RequiredResourceId => _boosterData.RequiredResourceId;

        public int DurationSeconds => _boosterData.DurationSeconds;
        
        public bool IsEnough => _boosterResource.IsEnough;

        public Sprite IconSprite =>
            _boosterData.IconSprite;

        public void ActivateBooster() =>
            ActivateBoosterAction?.Invoke(this);
    }
}