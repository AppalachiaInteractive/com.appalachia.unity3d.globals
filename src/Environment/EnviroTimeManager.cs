#region

using Appalachia.Editing.Attributes;
using Appalachia.Utility.Constants;
using Unity.Profiling;
using UnityEditor;
#if UNITY_EDITOR

#endif

#endregion

namespace Appalachia.Globals.Environment
{
    [EditorOnlyInitializeOnLoad]
    public static class EnviroTimeManager
    {
        private const string _PRF_PFX = nameof(EnviroTimeManager) + ".";
        public static bool breakTime = false;

        private static bool _paused;

        private static EnviroTime.TimeProgressMode _unpauseMode = EnviroTime.TimeProgressMode.Simulated;

        private static bool _initialized;
        private static EnviroSky _core;
        private static EnviroSkyMgr _mgr;

        

        public static bool Valid => (_core != null) && (_mgr != null);

        private static readonly ProfilerMarker _PRF_Initialize = new ProfilerMarker(_PRF_PFX + nameof(Initialize));
        
        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            using (_PRF_Initialize.Auto())
            {
                if (_core == null)
                {
                    _core = EnviroSky.instance;
                }

                if (_mgr == null)
                {
                    _mgr = EnviroSkyMgr.instance;
                }

                _initialized = true;
            }
        }

        public static void SetCycleTime(int minutes)
        {
            Initialize();
            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.cycleLengthInMinutes = minutes;

            RefreshScene();
        }

        public static void UpdateTimeProgression(EnviroTime.TimeProgressMode type)
        {
            Initialize();
            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.ProgressTime = type;

            RefreshScene();
        }

        public static void SetDate(int years, int days)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Years = years;
            time.Days = days;

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static void SetTime(int hours, int minutes, int seconds)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Hours = hours;
            time.Minutes = minutes;
            time.Seconds = seconds;

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static void SetHour(int hour)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Hours = hour;

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static void SetMinute(int minutes)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Minutes = minutes;

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static void SetDateAndTime(int years, int days, int hours, int minutes, int seconds)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Years = years;
            time.Days = days;
            time.Hours = hours;
            time.Minutes = minutes;
            time.Seconds = seconds;

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static void AddTime(int hours, int minutes, int seconds)
        {
            Initialize();

            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            time.Hours += hours;
            time.Minutes += minutes;
            time.Seconds += seconds;

            while (time.Seconds > 59)
            {
                if (breakTime)
                {
                    return;
                }

                time.Minutes += 1;
                time.Seconds -= 60;
            }

            while (time.Seconds < 0)
            {
                if (breakTime)
                {
                    return;
                }

                time.Minutes -= 1;
                time.Seconds += 60;
            }

            while (time.Minutes > 59)
            {
                if (breakTime)
                {
                    return;
                }

                time.Hours += 1;
                time.Minutes -= 60;
            }

            while (time.Minutes < 0)
            {
                if (breakTime)
                {
                    return;
                }

                time.Hours -= 1;
                time.Minutes += 60;
            }

            while (time.Hours > 23)
            {
                if (breakTime)
                {
                    return;
                }

                time.Days += 1;
                time.Hours -= 23;
            }

            while (time.Hours < 0)
            {
                if (breakTime)
                {
                    return;
                }

                time.Days -= 1;
                time.Hours += 23;
            }

            while (time.Days > time.DaysInYear)
            {
                if (breakTime)
                {
                    return;
                }

                time.Years += 1;
                time.Days -= time.DaysInYear;
            }

            while (time.Days < 0)
            {
                if (breakTime)
                {
                    return;
                }

                time.Years -= 1;
                time.Days += time.DaysInYear;
            }

            _core.SetTime(time.Years, time.Days, time.Hours, time.Minutes, time.Seconds);

            RefreshScene();
        }

        public static float SolarTime()
        {
            Initialize();

            return _core.GameTime.solarTime;
        }

        public static bool Morning()
        {
            Initialize();

            return _core.GameTime.Hours < 12;
        }

        public static float LunarTime()
        {
            return 1.0f - _core.GameTime.solarTime;
        }

        private static readonly ProfilerMarker _PRF_RefreshScene = new ProfilerMarker(_PRF_PFX + nameof(RefreshScene));
        private static void RefreshScene()
        {
            using (_PRF_RefreshScene.Auto())
            {
                _core.UpdateTime(_core.GameTime.DaysInYear);

#if UNITY_EDITOR
                EditorApplication.QueuePlayerLoopUpdate();
#endif
            }
        }

#if UNITY_EDITOR

        [MenuItem("Tools/Enviro/Time/Paused" + SHC.ALT_P, true)]
        public static bool ToggleEnviroTimePauseValidate()
        {
            Menu.SetChecked("Tools/Enviro/Time/Paused", _paused);
            return true;
        }

        [MenuItem("Tools/Enviro/Time/Paused" + SHC.ALT_P, priority = 1050)]
        public static void ToggleEnviroTimePause()
        {
            SetPaused(!_paused);
        }

        [MenuItem("Tools/Enviro/Time/Progress/None" + SHC.ALT_F1)]
        private static void SET_Progress_00()
        {
            SET_Progress(0);
        }

        [MenuItem("Tools/Enviro/Time/Progress/Simulated" + SHC.ALT_F2)]
        private static void SET_Progress_01()
        {
            SET_Progress(1);
        }

        [MenuItem("Tools/Enviro/Time/Progress/OneDay" + SHC.ALT_F3)]
        private static void SET_Progress_02()
        {
            SET_Progress(2);
        }

        [MenuItem("Tools/Enviro/Time/Progress/SystemTime" + SHC.ALT_F4)]
        private static void SET_Progress_03()
        {
            SET_Progress(3);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/1 minute" + SHC.CTRL_ALT_SHFT_1)]
        private static void SET_CycleTime_001()
        {
            SetCycleTime(001);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/2 minutes")]
        private static void SET_CycleTime_002()
        {
            SetCycleTime(002);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/5 minutes" + SHC.CTRL_ALT_SHFT_2)]
        private static void SET_CycleTime_005()
        {
            SetCycleTime(005);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/10 minutes")]
        private static void SET_CycleTime_010()
        {
            SetCycleTime(010);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/15 minutes" + SHC.CTRL_ALT_SHFT_3)]
        private static void SET_CycleTime_015()
        {
            SetCycleTime(015);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/20 minutes")]
        private static void SET_CycleTime_020()
        {
            SetCycleTime(020);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/30 minutes" + SHC.CTRL_ALT_SHFT_4)]
        private static void SET_CycleTime_030()
        {
            SetCycleTime(030);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/45 minutes")]
        private static void SET_CycleTime_045()
        {
            SetCycleTime(045);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/60 minutes" + SHC.CTRL_ALT_SHFT_5)]
        private static void SET_CycleTime_060()
        {
            SetCycleTime(060);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/90 minutes" + SHC.CTRL_ALT_SHFT_6)]
        private static void SET_CycleTime_090()
        {
            SetCycleTime(090);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/120 minutes" + SHC.CTRL_ALT_SHFT_7)]
        private static void SET_CycleTime_120()
        {
            SetCycleTime(120);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/150 minutes" + SHC.CTRL_ALT_SHFT_8)]
        private static void SET_CycleTime_150()
        {
            SetCycleTime(150);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/180 minutes" + SHC.CTRL_ALT_SHFT_9)]
        private static void SET_CycleTime_180()
        {
            SetCycleTime(180);
        }

        [MenuItem("Tools/Enviro/Time/Cycle Time/240 minutes" + SHC.CTRL_ALT_SHFT_0)]
        private static void SET_CycleTime_240()
        {
            SetCycleTime(240);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/12 AM (Midnight)" + SHC.CTRL_SHFT_0)]
        private static void SetHour_00()
        {
            SetHour(0);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/1 AM" + SHC.CTRL_SHFT_1)]
        private static void SetHour_01()
        {
            SetHour(1);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/2 AM" + SHC.CTRL_SHFT_2)]
        private static void SetHour_02()
        {
            SetHour(2);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/3 AM" + SHC.CTRL_SHFT_3)]
        private static void SetHour_03()
        {
            SetHour(3);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/4 AM" + SHC.CTRL_SHFT_4)]
        private static void SetHour_04()
        {
            SetHour(4);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/5 AM" + SHC.CTRL_SHFT_5)]
        private static void SetHour_05()
        {
            SetHour(5);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/6 AM" + SHC.CTRL_SHFT_6)]
        private static void SetHour_06()
        {
            SetHour(6);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/7 AM" + SHC.CTRL_SHFT_7)]
        private static void SetHour_07()
        {
            SetHour(7);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/8 AM" + SHC.CTRL_SHFT_8)]
        private static void SetHour_08()
        {
            SetHour(8);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/9 AM" + SHC.CTRL_SHFT_9)]
        private static void SetHour_09()
        {
            SetHour(9);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/10 AM")]
        private static void SetHour_10()
        {
            SetHour(10);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/AM/11 AM")]
        private static void SetHour_11()
        {
            SetHour(11);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/12 PM (Noon)" + SHC.ALT_SHFT_0)]
        private static void SetHour_12()
        {
            SetHour(12);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/1 PM" + SHC.ALT_SHFT_1)]
        private static void SetHour_13()
        {
            SetHour(13);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/2 PM" + SHC.ALT_SHFT_2)]
        private static void SetHour_14()
        {
            SetHour(14);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/3 PM" + SHC.ALT_SHFT_3)]
        private static void SetHour_15()
        {
            SetHour(15);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/4 PM" + SHC.ALT_SHFT_4)]
        private static void SetHour_16()
        {
            SetHour(16);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/5 PM" + SHC.ALT_SHFT_5)]
        private static void SetHour_17()
        {
            SetHour(17);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/6 PM" + SHC.ALT_SHFT_6)]
        private static void SetHour_18()
        {
            SetHour(18);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/7 PM" + SHC.ALT_SHFT_7)]
        private static void SetHour_19()
        {
            SetHour(19);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/8 PM" + SHC.ALT_SHFT_8)]
        private static void SetHour_20()
        {
            SetHour(20);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/9 PM" + SHC.ALT_SHFT_9)]
        private static void SetHour_21()
        {
            SetHour(21);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/10 PM")]
        private static void SetHour_22()
        {
            SetHour(22);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Hour/PM/11 PM")]
        private static void SetHour_23()
        {
            SetHour(23);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/0")]
        private static void SetMinute_00()
        {
            SetMinute(0);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/5")]
        private static void SetMinute_05()
        {
            SetMinute(5);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/10")]
        private static void SetMinute_10()
        {
            SetMinute(10);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/15")]
        private static void SetMinute_15()
        {
            SetMinute(15);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/20")]
        private static void SetMinute_20()
        {
            SetMinute(20);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/25")]
        private static void SetMinute_25()
        {
            SetMinute(25);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/30")]
        private static void SetMinute_30()
        {
            SetMinute(30);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/35")]
        private static void SetMinute_35()
        {
            SetMinute(35);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/40")]
        private static void SetMinute_40()
        {
            SetMinute(40);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/45")]
        private static void SetMinute_45()
        {
            SetMinute(45);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/50")]
        private static void SetMinute_50()
        {
            SetMinute(50);
        }

        [MenuItem("Tools/Enviro/Time/Set Time/Minute/55")]
        private static void SetMinute_55()
        {
            SetMinute(55);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/1" + SHC.CTRL_PageUp)]
        private static void SET_MoveTime_01H()
        {
            SET_Move(01, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/2")]
        private static void SET_MoveTime_02H()
        {
            SET_Move(02, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/3")]
        private static void SET_MoveTime_03H()
        {
            SET_Move(03, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/4")]
        private static void SET_MoveTime_04H()
        {
            SET_Move(04, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/6" + SHC.CTRL_Home)]
        private static void SET_MoveTime_06H()
        {
            SET_Move(06, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/9")]
        private static void SET_MoveTime_09H()
        {
            SET_Move(09, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/12" + SHC.CTRL_Insert)]
        private static void SET_MoveTime_12H()
        {
            SET_Move(12, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/16")]
        private static void SET_MoveTime_16H()
        {
            SET_Move(16, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/18")]
        private static void SET_MoveTime_18H()
        {
            SET_Move(18, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Hour/20")]
        private static void SET_MoveTime_20H()
        {
            SET_Move(20, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/1" + SHC.ALT_PageUp)]
        private static void SET_MoveTime_01M()
        {
            SET_Move(00, 01, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/2")]
        private static void SET_MoveTime_02M()
        {
            SET_Move(00, 02, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/5")]
        private static void SET_MoveTime_05M()
        {
            SET_Move(00, 05, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/10")]
        private static void SET_MoveTime_10M()
        {
            SET_Move(00, 10, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/15" + SHC.ALT_Home)]
        private static void SET_MoveTime_15M()
        {
            SET_Move(00, 15, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/20")]
        private static void SET_MoveTime_20M()
        {
            SET_Move(00, 20, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/30" + SHC.ALT_Insert)]
        private static void SET_MoveTime_30M()
        {
            SET_Move(00, 30, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Forward/Minute/45")]
        private static void SET_MoveTime_45M()
        {
            SET_Move(00, 45, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/1" + SHC.CTRL_PageDown)]
        private static void SET_MoveTime_N01H()
        {
            SET_Move(-01, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/2")]
        private static void SET_MoveTime_N02H()
        {
            SET_Move(-02, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/3")]
        private static void SET_MoveTime_N03H()
        {
            SET_Move(-03, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/4")]
        private static void SET_MoveTime_N04H()
        {
            SET_Move(-04, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/6" + SHC.CTRL_End)]
        private static void SET_MoveTime_N06H()
        {
            SET_Move(-06, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/9")]
        private static void SET_MoveTime_N09H()
        {
            SET_Move(-09, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/12" + SHC.CTRL_Delete)]
        private static void SET_MoveTime_N12H()
        {
            SET_Move(-12, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/16")]
        private static void SET_MoveTime_N16H()
        {
            SET_Move(-16, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/18")]
        private static void SET_MoveTime_N18H()
        {
            SET_Move(-18, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Hour/20")]
        private static void SET_MoveTime_N20H()
        {
            SET_Move(-20, 00, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/1" + SHC.ALT_PageDown)]
        private static void SET_MoveTime_N01M()
        {
            SET_Move(00, -01, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/2")]
        private static void SET_MoveTime_N02M()
        {
            SET_Move(00, -02, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/5")]
        private static void SET_MoveTime_N05M()
        {
            SET_Move(00, -05, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/10")]
        private static void SET_MoveTime_N10M()
        {
            SET_Move(00, -10, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/15" + SHC.ALT_End)]
        private static void SET_MoveTime_N15M()
        {
            SET_Move(00, -15, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/20")]
        private static void SET_MoveTime_N20M()
        {
            SET_Move(00, -20, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/30" + SHC.ALT_Delete)]
        private static void SET_MoveTime_N30M()
        {
            SET_Move(00, -30, 00);
        }

        [MenuItem("Tools/Enviro/Time/Move Time/Back/Minute/45")]
        private static void SET_MoveTime_N45M()
        {
            SET_Move(00, -45, 00);
        }

        private static void SET_Progress(int type)
        {
            switch (type)
            {
                case 0:
                    UpdateTimeProgression(EnviroTime.TimeProgressMode.None);
                    break;
                case 1:
                    UpdateTimeProgression(EnviroTime.TimeProgressMode.Simulated);
                    break;
                case 2:
                    UpdateTimeProgression(EnviroTime.TimeProgressMode.OneDay);
                    break;
                case 3:
                    UpdateTimeProgression(EnviroTime.TimeProgressMode.SystemTime);
                    break;
            }
        }

        private static void SetPaused(bool pause)
        {
            Initialize();
            if ((_core == null) || !_core.enabled)
            {
                return;
            }

            var time = _core.GameTime;

            if (pause)
            {
                time.ProgressTime = _unpauseMode;
            }
            else
            {
                _unpauseMode = time.ProgressTime;
                time.ProgressTime = EnviroTime.TimeProgressMode.None;
            }

            RefreshScene();

            _paused = pause;
        }

        private static void SET_Move(int hour, int minute, int second)
        {
            AddTime(hour, minute, second);
        }

#endif
    }
}
