using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "ResourceCostData", menuName = "ScriptableObjects/CreateResourceCostData")]
    public class ResourceCost : ScriptableObject
    {
        public int ResourceId;
        public int Amount;
    }
}