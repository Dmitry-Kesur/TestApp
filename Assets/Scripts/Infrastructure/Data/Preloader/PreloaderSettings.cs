using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Data.Preloader
{
    [CreateAssetMenu(fileName = "PreloaderSettings", menuName = "ScriptableObjects/PreloaderSettings")]
    public class PreloaderSettings : ScriptableObject
    {
        [SerializeField] private List<PreloaderData> _preloaderDatas;

        public PreloaderData GetData(LoadingStage loadingStage)
        {
            return _preloaderDatas.FirstOrDefault(preloaderData => preloaderData.LoadingStage == loadingStage);
        }
    }

    [Serializable]
    public class PreloaderData
    {
        public LoadingStage LoadingStage;
        public int SortValue;
        public float ProgressValue;
        public string StageText;
    }

    public enum LoadingStage
    {
        LoadingGameItems,
        LoadingGameLevels,
        LoadingProducts,
        LoadingSounds,
        LoadingRewards,
        LoadingBoosters,
        LoadingFirebase
    }
}