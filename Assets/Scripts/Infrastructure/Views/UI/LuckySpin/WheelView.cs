using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Models.UI;
using UnityEngine;

namespace Infrastructure.Views.UI
{
    public class WheelView : MonoBehaviour
    {
        public event Action<int> OnCompleteSpinAction; 
        
        [SerializeField] private List<WheelSegmentView> _wheelSegments;

        public void DrawSegments(List<WheelSegmentModel> segmentModels)
        {
            for (int i = 0; i < segmentModels.Count; i++)
            {
                var segmentView = _wheelSegments[i];
                var segmentModel = segmentModels[i];
                segmentView.SetModel(segmentModel);
                segmentView.Draw();
            }
        }

        public void StartSpin(int segmentIndex)
        {
            float seg = 360f / _wheelSegments.Count;
            float center = seg * segmentIndex;

            int spins = 6;
            float duration = 2.2f;
            
            float final = center - 360f * spins;

            transform.DOLocalRotate(new Vector3(0, 0, final), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    OnCompleteSpinAction?.Invoke(segmentIndex); 
                });
        }
    }
}