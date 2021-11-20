#region

using Appalachia.Core.Behaviours;
using Appalachia.Core.Timing;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Timing
{
    [ExecuteAlways]
    public class CoreClockTicker : SingletonAppalachiaBehaviour<CoreClockTicker>
    {
        private void FixedUpdate()
        {
            CoreClock.Tick();
        }

        protected override void Start()
        {
            base.Start();
            
            CoreClock.Tick();
        }

        private void Update()
        {
            CoreClock.Tick();
        }
    }
}
