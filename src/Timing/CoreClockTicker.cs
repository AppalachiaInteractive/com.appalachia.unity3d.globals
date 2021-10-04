#region

using Appalachia.Core.Behaviours;

#endregion

namespace Appalachia.Core.Globals.Timing
{
    public class CoreClockTicker : InternalMonoBehaviour
    {
        private void Start()
        {
            CoreClock.Tick();
        }

        private void Update()
        {
            CoreClock.Tick();
        }

        private void FixedUpdate()
        {
            CoreClock.Tick();
        }
    }
}
