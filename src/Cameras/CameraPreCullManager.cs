#region

using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Globals.Cameras
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class CameraPreCullManager: InternalMonoBehaviour
    {
        private const string _PRF_PFX = nameof(CameraPreCullManager) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + "Awake");
        private static readonly ProfilerMarker _PRF_Start = new ProfilerMarker(_PRF_PFX + "Start");
        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + "OnEnable");
        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + "Update");
        private static readonly ProfilerMarker _PRF_LateUpdate = new ProfilerMarker(_PRF_PFX + "LateUpdate");
        private static readonly ProfilerMarker _PRF_OnDisable = new ProfilerMarker(_PRF_PFX + "OnDisable");
        private static readonly ProfilerMarker _PRF_OnDestroy = new ProfilerMarker(_PRF_PFX + "OnDestroy");
        private static readonly ProfilerMarker _PRF_Reset = new ProfilerMarker(_PRF_PFX + "Reset");
        private static readonly ProfilerMarker _PRF_OnDrawGizmos = new ProfilerMarker(_PRF_PFX + "OnDrawGizmos");
        private static readonly ProfilerMarker _PRF_OnDrawGizmosSelected = new ProfilerMarker(_PRF_PFX + "OnDrawGizmosSelected");
        
        public Camera _cam;
        public event CameraPreCull OnCameraPreCull;

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                _cam = GetComponent<Camera>();
            }
        }

        private static readonly ProfilerMarker _PRF_OnPreCull = new ProfilerMarker(_PRF_PFX + nameof(OnPreCull));
        private void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                OnCameraPreCull?.Invoke(_cam);
            }
        }
    }
}
