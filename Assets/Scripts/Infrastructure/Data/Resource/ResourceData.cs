using Infrastructure.Constants;
using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/CreateResourceData")]
    public class ResourceData : ScriptableObject
    {
        public int Id;
        public string Name;
        public ResourceType Type;
        public Sprite Icon;
    }
}