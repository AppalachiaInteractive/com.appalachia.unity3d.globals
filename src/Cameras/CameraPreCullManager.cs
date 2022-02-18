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
        public event CameraPreCull OnCameraPreCull;

        #region Fields and Autoproperties

        public Camera _cam;

        #endregion

        #region Event Functions

        private void OnPreCull()
        {
            using (_PRF_OnPreCull.Auto())
            {
                OnCameraPreCull?.Invoke(_cam);
            }
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _cam = GetComponent<Camera>();
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnPreCull = new(_PRF_PFX + nameof(OnPreCull));

        #endregion

#if UNITY_EDITOR

#endif
    }
}
