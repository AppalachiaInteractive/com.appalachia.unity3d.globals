using System;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Appalachia.Globals.Application.Scenes
{
    [Serializable]
    public class SceneBootloadProgress
    {
        public AsyncOperationHandle<SceneInstance> operation;
        public SceneInstance scene;
        [ShowInInspector] public float progress => operation.PercentComplete;
    }
}
