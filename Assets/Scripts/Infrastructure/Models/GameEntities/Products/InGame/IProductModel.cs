using System;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Products.InGame
{
    public interface IProductModel
    {
        int Price { get; }
        int Id { get; }
        Sprite ProductIcon { get; }
        IPurchaseProductTarget GetPurchaseTarget();
        Action UpdateProductAction { get; set; }
    }
}