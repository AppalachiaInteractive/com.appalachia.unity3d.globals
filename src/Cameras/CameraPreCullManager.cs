#region

using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Cameras
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class CameraPreCullManager : AppalachiaBehaviour<CameraPreCullManager>
    {
        #region Fields and Autoproperties

        public Camera _cam;

        #endregion

        public event CameraPreCull OnCameraPreCull;

        #region Event Functions

        private void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                OnCameraPreCull?.Invoke(_cam);
            }
        }

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                _cam = GetComponent<Camera>();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CameraPreCullManager) + ".";

        private static readonly ProfilerMarker _PRF_OnPreCull = new(_PRF_PFX + nameof(OnPreCull));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR

#endif
    }
}
