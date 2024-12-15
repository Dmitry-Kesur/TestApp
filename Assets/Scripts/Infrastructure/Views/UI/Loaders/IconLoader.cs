using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Loaders
{
    public class IconLoader : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        
        public void SetIconSprite(Sprite sprite)
        {
            _iconImage.sprite = sprite;
        }
    }
}