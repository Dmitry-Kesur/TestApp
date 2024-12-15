using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Infrastructure.Data.PlayerProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public int ActiveLevel;
        public int BestScore;
        public int SelectedItemId;
        public int CurrencyAmount;
        public int ActiveBoosterId;
        public string UserId;
        public bool MuteSounds;
        public List<int> CompleteLevelIds = new List<int>();
        public List<int> UnlockedLevelItemIds = new List<int>();
        public List<int> PurchasedProductIds = new List<int>();
    }
}