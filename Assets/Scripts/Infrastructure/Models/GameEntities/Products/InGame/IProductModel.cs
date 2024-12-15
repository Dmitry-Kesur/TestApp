using UnityEngine;

namespace Infrastructure.Models.GameEntities.Products.InGame
{
    public interface IProductModel
    {
        int Price { get; }
        int Id { get; }
        Sprite ProductIcon { get; }
        void OnPurchaseComplete();
    }
}