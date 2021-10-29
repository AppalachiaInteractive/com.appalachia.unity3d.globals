namespace Appalachia.Globals.Environment
{
    public static class ENVIRONMENT
    {
        public static float lunarTime => EnviroTimeManager.LunarTime();
        public static float solarTime => EnviroTimeManager.SolarTime();
    }
}
