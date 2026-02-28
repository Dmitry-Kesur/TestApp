using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure.Data.Products
{
    [CreateAssetMenu(fileName = "PurchaseRewardData", menuName = "ScriptableObjects/CreatePurchaseReward")]
    public class PurchaseReward : ScriptableObject
    {
        public PurchaseRewardType Type;
        public int Id;
        public int Amount;
    }
}