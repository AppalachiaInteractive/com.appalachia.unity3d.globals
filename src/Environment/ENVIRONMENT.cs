namespace Appalachia.Core.Globals.Environment
{
    public static class ENVIRONMENT
    {
        public static float solarTime => EnviroTimeManager.SolarTime();
        
        public static float lunarTime => EnviroTimeManager.LunarTime();
    }
}
