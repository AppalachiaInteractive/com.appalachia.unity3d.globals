#region

using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Timing;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Timing
{
    [ExecuteAlways]
    public class CoreClockTicker : SingletonAppalachiaBehaviour<CoreClockTicker>
    {
        #region Event Functions

        private void Update()
        {
            CoreClock.Tick();
        }

        private void FixedUpdate()
        {
            CoreClock.Tick();
        }

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                CoreClock.Tick();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CoreClockTicker) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
