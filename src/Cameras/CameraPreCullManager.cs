#region

using Appalachia.Core.Behaviours;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Cameras
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class CameraPreCullManager : AppalachiaBehaviour
    {
        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(CameraPreCullManager) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + "Awake");
        private static readonly ProfilerMarker _PRF_Start = new(_PRF_PFX + "Start");
        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + "OnEnable");
        private static readonly ProfilerMarker _PRF_Update = new(_PRF_PFX + "Update");
        private static readonly ProfilerMarker _PRF_LateUpdate = new(_PRF_PFX + "LateUpdate");
        private static readonly ProfilerMarker _PRF_OnDisable = new(_PRF_PFX + "OnDisable");
        private static readonly ProfilerMarker _PRF_OnDestroy = new(_PRF_PFX + "OnDestroy");

        private static readonly ProfilerMarker _PRF_Reset = new(_PRF_PFX + "Reset");
        private static readonly ProfilerMarker _PRF_OnDrawGizmos = new(_PRF_PFX + "OnDrawGizmos");

        private static readonly ProfilerMarker _PRF_OnDrawGizmosSelected =
            new(_PRF_PFX + "OnDrawGizmosSelected");

        private static readonly ProfilerMarker _PRF_OnPreCull = new(_PRF_PFX + nameof(OnPreCull));

        #endregion

        public Camera _cam;

        public event CameraPreCull OnCameraPreCull;

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                _cam = GetComponent<Camera>();
            }
        }

        private void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                OnCameraPreCull?.Invoke(_cam);
            }
        }
    }
}
