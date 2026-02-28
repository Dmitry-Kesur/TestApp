using System;
using Infrastructure.Constants;
using Infrastructure.Data;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Resources
{
    public class ResourceModel
    {
        private int _amount;
        
        private ResourceData _resourceData;

        public void SetData(ResourceData resourceData) =>
            _resourceData = resourceData;

        public int Id => _resourceData.Id;

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnAmountChange?.Invoke();
            }
        }

        public bool IsEnough => _amount > 0;
        
        public ResourceType Type => _resourceData.Type;

        public Sprite Icon => _resourceData.Icon;


        public event Action OnAmountChange;
    }
}