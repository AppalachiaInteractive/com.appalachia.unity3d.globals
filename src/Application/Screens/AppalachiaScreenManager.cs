using System;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Behaviours;
using Appalachia.Editing.Debugging.Graphy;
using Appalachia.Editing.Debugging.IngameDebugConsole;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Appalachia.Globals.Application.Screens
{
    public class AppalachiaScreenManager : SingletonAppalachiaBehaviour<AppalachiaScreenManager>
    {
        private void DisableInput()
        {
#if UNITY_EDITOR
            graphyToggleActive.action.Disable();
            graphyToggleModes.action.Disable();
            debugLogToggle.action.Disable();
            captureScreenshot.action.Disable();
#endif
        }

        private void EnableInput()
        {
#if UNITY_EDITOR
            graphyToggleActive.action.Enable();
            graphyToggleModes.action.Enable();
            debugLogToggle.action.Enable();
            captureScreenshot.action.Enable();

            graphyToggleActive.action.performed += _ => { GraphyManager.instance.ToggleActive(); };
            graphyToggleModes.action.performed += _ => { GraphyManager.instance.ToggleModes(); };
            debugLogToggle.action.performed += _ => { DebugLogManager.instance.Toggle(); };
            captureScreenshot.action.performed += _ =>
            {
                var now = DateTime.Now;
                var filename =
                    $"{SceneManager.GetSceneAt(1).name}-{now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}{now.Second:D2}.png";
                var filePath = AppaPath.Combine("Screenshots", filename);

                AppaDirectory.CreateDirectoryStructureForFilePath(filePath);
                
                AppaLog.Info($"Captured Screenshot to : {filePath}");
                ScreenCapture.CaptureScreenshot(filePath);
            };
#endif
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            if (graphyInstance == null)
            {
                graphyInstance = Instantiate(graphyPrefab);
            }

            if (inGameConsoleInstance == null)
            {
                inGameConsoleInstance = Instantiate(inGameConsolePrefab);
            }
#endif
        }
#if UNITY_EDITOR

        [BoxGroup("Editor Only/Graphy")]
        [ReadOnly]
        public GameObject graphyInstance;

        [FoldoutGroup("Editor Only")]
        [BoxGroup("Editor Only/Graphy")]
        public GameObject graphyPrefab;

        [BoxGroup("Editor Only/Debug Log")]
        [ReadOnly]
        public GameObject inGameConsoleInstance;

        [BoxGroup("Editor Only/Debug Log")]
        public GameObject inGameConsolePrefab;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference captureScreenshot;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference debugLogToggle;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference graphyToggleActive;

        [BoxGroup("Editor Only/Input")]
        public InputActionReference graphyToggleModes;

#endif

        #region Event Functions

        private void OnEnable()
        {
            Initialize();
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        #endregion
    }
}
