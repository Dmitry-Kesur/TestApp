using Infrastructure.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Infrastructure.Data.Items
{
    public class ItemData : ScriptableObject
    {
        public int Id;
        public float SpawnChance;
        public ItemsType ItemType;
        [PreviewField(ObjectFieldAlignment.Left)]
        public Sprite Sprite;
        [PropertyRange(0, 50)] public int ScorePoints;
    }   
}