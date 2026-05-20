using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Models.UI;
using UnityEngine;

namespace Infrastructure.Views.UI
{
    public class WheelView : MonoBehaviour
    {
        private const int FakeSpinCount = 6;
        private const int FullSpinValue = 360;
        private const float SpinDuration = 2.2f;

        private Tween _spinTween;
        
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
            int segmentStep = FullSpinValue / _wheelSegments.Count;
            float center = segmentStep * segmentIndex;
            
            float finalSpinValue = center - FullSpinValue * FakeSpinCount;

            _spinTween = transform.DOLocalRotate(new Vector3(0, 0, finalSpinValue), SpinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    OnCompleteSpinAction?.Invoke(segmentIndex); 
                });
        }

        private void KillTween()
        {
            _spinTween?.Kill();
            _spinTween = null;
        }

        private void OnDisable()
        {
            KillTween();
        }
    }
}