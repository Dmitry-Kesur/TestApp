using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure.Data.Products
{
    [CreateAssetMenu(fileName = "ProductData", menuName = "ScriptableObjects/CreateProductData")]
    public class ProductData : ScriptableObject
    {
        public int ProductId;
        public int Price;
        public Sprite Icon;
        public ProductCategory ProductCategory;
    }
}