#region

using Appalachia.Core.Behaviours;
using Appalachia.Core.Timing;

#endregion

namespace Appalachia.Globals.Timing
{
    public class CoreClockTicker : AppalachiaMonoBehaviour
    {
        private void FixedUpdate()
        {
            CoreClock.Tick();
        }

        private void Start()
        {
            CoreClock.Tick();
        }

        private void Update()
        {
            CoreClock.Tick();
        }
    }
}
