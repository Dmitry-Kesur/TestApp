using System.Collections.Generic;
using Infrastructure.Models.UI.Items;
using Infrastructure.Views.UI.Items;
using UnityEngine;

namespace Infrastructure.Views.UI.Loaders
{
    public class LoaderWithCache<T> : MonoBehaviour where T: IDrawableModel 
    {
        [SerializeField] protected List<DrawableItem<T>> cachedItems;

        public void DrawLoader(List<T> loaderData)
        {
            HideItems();
            
            for (int i = 0; i < loaderData.Count; i++)
            {
                var levelModel = loaderData[i];
                var levelSelectItem = cachedItems[i];
                levelSelectItem.gameObject.SetActive(true);
                levelSelectItem.SetModel(levelModel);
                levelSelectItem.Draw();
                SubscribeItemListeners(levelSelectItem);
            }
        }

        protected virtual void SubscribeItemListeners(DrawableItem<T> drawableItem)
        {
            
        }

        private void HideItems()
        {
            foreach (var cachedItem in cachedItems)
            {
                cachedItem.gameObject.SetActive(false);
            }
        }
    }
}