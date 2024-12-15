using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.HUD
{
    public class FailItemsView : MonoBehaviour
    {
        [SerializeField] private List<Image> _failMarks;
        private int _currentFailMarkIndex;

        public void UpdateFailItems(int failItemsAmount)
        {
            if (failItemsAmount == 0)
            {
                HideFailMarks();
                return;
            }

            for (int i = 0; i < failItemsAmount; i++)
            {
                var failMark = _failMarks[i];
                if (failMark != null)
                {
                    ChangeFailMarkVisibility(failMark, true);
                }
            }
        }

        private void ChangeFailMarkVisibility(Image failMark, bool visibility)
        {
            failMark.gameObject.SetActive(visibility);
        }

        private void HideFailMarks()
        {
            foreach (var failMark in _failMarks)
            {
                ChangeFailMarkVisibility(failMark, false);
            }
        }
    }
}