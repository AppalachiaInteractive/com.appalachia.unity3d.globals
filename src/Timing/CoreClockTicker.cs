#region

using Appalachia.Base.Behaviours;

#endregion

namespace Appalachia.Globals.Timing
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
