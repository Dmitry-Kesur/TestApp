using Infrastructure.Data.Level;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelsEditor : OdinMenuEditorWindow
    {
        [MenuItem("Editors/Levels Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<LevelsEditor>();
            window.minSize = new Vector2(780, 340);
            window.maxSize = new Vector2(780, 340);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
    
            tree.Add("Create New Level", new CreateNewItemData());
            tree.AddAllAssetsAtPath("Levels", "Assets/Scripts/ScriptableObjects/Levels/", typeof(LevelStaticData));
            tree.UpdateMenuTree();

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            OdinMenuTreeSelection selected = MenuTree.Selection;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton("Delete level"))
                {
                    LevelStaticData asset = selected.SelectedValue as LevelStaticData;
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public class CreateNewItemData
        {
            public CreateNewItemData()
            {
                LevelStaticData = CreateInstance<LevelStaticData>();
                LevelStaticData.Level = 0;
            }

            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public LevelStaticData LevelStaticData;

            [Button("Add New Level")]
            private void CreateNewData()
            {
                AssetDatabase.CreateAsset(LevelStaticData,
                    "Assets/Scripts/ScriptableObjects/Levels/" + $"Level{LevelStaticData.Level}" + ".asset");
                AssetDatabase.SaveAssets();

                LevelStaticData = CreateInstance<LevelStaticData>();
                LevelStaticData.Level = 0;
            }
        }
    }
}