using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class DailyBonusWindow : BaseWindow
    {
        [SerializeField] private DaysLoader _daysLoader;

        private DailyBonusWindowModel _dailyBonusWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _dailyBonusWindowModel = model as DailyBonusWindowModel;
        }

        protected override void Draw()
        {
            base.Draw();
            _daysLoader.DrawLoader(_dailyBonusWindowModel.DayModels);
        }

        public override Type GetWindowControllerType() => typeof(DailyBonusWindowController);
    }
}