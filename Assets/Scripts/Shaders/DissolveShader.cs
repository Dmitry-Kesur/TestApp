using UnityEngine;
using UnityEngine.UI;

namespace Shaders
{
    public class DissolveShader : MonoBehaviour
    {
        private static readonly int DissolveAmountProperty = Shader.PropertyToID("_DissolveAmount");
        private static readonly int TargetTexture = Shader.PropertyToID("_MainTex");
        
        [SerializeField] private Image _dissolveTargetImage;

        public void UpdateDissolveShader()
        {
            var newMaterial = Instantiate(_dissolveTargetImage.material);
            _dissolveTargetImage.material = newMaterial;
            
            var targetTexture = _dissolveTargetImage.mainTexture;
            var targetImageMaterial = _dissolveTargetImage.material;
            targetImageMaterial.SetTexture(TargetTexture, targetTexture);
        }

        public float DissolveAmount
        {
            get => _dissolveTargetImage.material.GetFloat(DissolveAmountProperty);
            set => _dissolveTargetImage.material.SetFloat(DissolveAmountProperty, value);
        }
    }
}