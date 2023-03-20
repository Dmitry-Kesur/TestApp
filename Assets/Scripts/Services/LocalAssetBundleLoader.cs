using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public static class GameAssetBundles
    {
        public const string ItemSprites = "itemssprites";
    }
    
    public static class LocalAssetBundleLoader
    {
        public static Sprite[] LoadSpritesBundle(string bundleName)
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
            return assetBundle.LoadAllAssets<Sprite>();
        }
    }
}