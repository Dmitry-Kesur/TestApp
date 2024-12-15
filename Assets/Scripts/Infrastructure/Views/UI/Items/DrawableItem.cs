using System;
using Infrastructure.Models.UI.Items;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Items
{
    public class DrawableItem<T> : MonoBehaviour where T: IDrawableModel
    {
        [SerializeField] private IconLoader _iconLoader;
        
        protected T drawableModel;
        
        public virtual void SetModel(T drawableModel)
        {
            this.drawableModel = drawableModel;
        }

        public virtual void Draw()
        {
            DrawIcon();
        }

        protected virtual void Clear()
        {
            
        }

        private void DrawIcon()
        {
            if (_iconLoader == null)
                return;
            
            _iconLoader.SetIconSprite(drawableModel.IconSprite);
        }

        private void OnDisable()
        {
            Clear();
        }
    }
}