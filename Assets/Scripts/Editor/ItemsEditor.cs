using Infrastructure.Data.Items;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ItemsEditor : OdinMenuEditorWindow
    {
        [MenuItem("Editors/Items Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<ItemsEditor>();
            window.minSize = new Vector2(600, 200);
            window.maxSize = new Vector2(600, 200);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            tree.Add("Create New", new CreateNewItemData());
            tree.AddAllAssetsAtPath("Items", "Assets/Scripts/ScriptableObjects/Items/", typeof(ItemData));
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

                if (SirenixEditorGUI.ToolbarButton("Delete current"))
                {
                    ItemData asset = selected.SelectedValue as ItemData;
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
                itemData = CreateInstance<ItemData>();
                itemData.Id = 0;
            }

            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public ItemData itemData;

            [Button("Add New Item")]
            private void CreateNewData()
            {
                AssetDatabase.CreateAsset(itemData,
                    "Assets/Scripts/ScriptableObjects/Items/" + $"Item{itemData.Id}" + ".asset");
                AssetDatabase.SaveAssets();

                itemData = CreateInstance<ItemData>();
                itemData.Id = 0;
            }
        }
    }
}