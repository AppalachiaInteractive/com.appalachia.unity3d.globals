using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Appalachia.Globals.Application.Scenes
{
    public class SceneReference : AppalachiaObject<SceneReference>
    {
        public bool setActiveOnLoad;
        
        [ReadOnly] public AssetReference sceneReference;
        
#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateSelection))]
        public UnityEditor.SceneAsset sceneAsset;

        public void SetSelection(UnityEditor.SceneAsset asset)
        {
            sceneAsset = asset;
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var guid, out long _);

            sceneReference = new AssetReference(guid);
            SetDirty();
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(SceneReference),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew();
        }
#endif
    }
}
