using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace Appalachia.Globals.Application
{
    public class SceneReference : SelfSavingScriptableObject<SceneReference>
    {
        [ReadOnly] public AssetReference sceneReference;
#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdateSelection))]
        public UnityEditor.SceneAsset sceneAsset;

        private void UpdateSelection()
        {
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sceneAsset, out var guid, out long _);

            sceneReference = new AssetReference(guid);
            this.SetDirty();
        }
#endif
    }
}
