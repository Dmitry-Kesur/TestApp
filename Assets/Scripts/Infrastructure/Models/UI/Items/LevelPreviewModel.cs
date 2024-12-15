using Infrastructure.Data.Level;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.UI
{
    public class LevelPreviewModel : IDrawableModel
    {
        private readonly LevelStaticData _levelStaticData;

        public LevelPreviewModel(LevelStaticData levelStaticData)
        {
            _levelStaticData = levelStaticData;
        }

        public int Level =>
            _levelStaticData.Level;

        public bool IsActive { get; set; }

        public bool IsComplete { get; set; }
        
        public Sprite IconSprite { get; }
    }
}