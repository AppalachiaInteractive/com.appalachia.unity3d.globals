#region

using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Timing;
using Appalachia.Utility.Async;
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
            if (ShouldSkipUpdate)
            {
                return;
            }

            CoreClock.Tick();
        }

        private void FixedUpdate()
        {
            CoreClock.Tick();
        }

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                CoreClock.Tick();
            }
        }

    }
}
