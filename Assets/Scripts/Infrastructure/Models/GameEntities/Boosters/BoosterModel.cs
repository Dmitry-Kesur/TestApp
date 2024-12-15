using System;
using Infrastructure.Data.Boosters;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Boosters
{
    public class BoosterModel : IDrawableModel
    {
        private readonly BoosterData _boosterData;

        public Action<string> OnBuyBoosterAction;

        public BoosterModel(BoosterData boosterData)
        {
            _boosterData = boosterData;
        }
        
        public int Id =>
            _boosterData.Id;

        public int BoostValue =>
            _boosterData.BoostValue;
        
        public Sprite IconSprite =>
            _boosterData.IconSprite;

        public string ProductId =>
            _boosterData.ProductId;

        public void OnBuyBooster() =>
            OnBuyBoosterAction?.Invoke(ProductId);
    }
}