using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class PreviewSelectItem : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void SetSprite(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}