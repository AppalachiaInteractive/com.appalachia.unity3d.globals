using System.Collections;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Core.Extensions;
using Appalachia.Globals.Application.Scenes;
using Appalachia.Globals.Application.Screens;
using Appalachia.Globals.Application.Screens.Fading;
using Appalachia.Globals.Application.State;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Appalachia.Globals.Application
{
    [AlwaysInitializeOnLoad]
    public class ApplicationManager : SingletonAppalachiaBehaviour<ApplicationManager>
    {
        static ApplicationManager()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => instance.Initialize();
#endif
        }

        [Title("Application State"), InlineProperty, HideLabel]
        public ApplicationState state;

        private AppalachiaScreenManager _screenManager;

        private bool _isApplicationFocused;
        private bool _isSceneLoading;
        private FrameEnd _frameEnd;
        private FrameStart _frameStart;

        [Title("Bootload Data")]
        [SerializeField]
        private SceneBootloadDataCollection _bootloads;

        private ScreenFadeManager _screenFader;

        public bool IsApplicationFocused => _isApplicationFocused;

        public bool IsNextStateReady
        {
            get
            {
                if (state.next == null)
                {
                    return false;
                }

                if (state.next.substate != ApplicationStates.LoadComplete)
                {
                    return false;
                }

                return true;
            }
        }

        public AppalachiaScreenManager ScreenManager
        {
            get => _screenManager;
            set => _screenManager = value;
        }

        public bool IsSceneLoading
        {
            get => _isSceneLoading;
            set => _isSceneLoading = value;
        }

        public IEnumerator BootloadScenes(SceneBootloadData bootloadData)
        {
            if (bootloadData.bootloads == null)
            {
                bootloadData.bootloads = new List<SceneBootloadProgress>();
            }

            var sceneReferences = bootloadData.GetScenesToLoad();

            foreach (var sceneReference in sceneReferences)
            {
                var assetReference = sceneReference.sceneReference;

                var bootload = new SceneBootloadProgress
                {
                    operation = assetReference.LoadSceneAsync(LoadSceneMode.Additive, false)
                };

                bootload.operation.Completed += handle => bootload.scene = handle.Result;

                bootloadData.bootloads.Add(bootload);
                yield return null;
            }
        }

        public void ExitApplication()
        {
            UnityEngine.Application.Quit();
        }

        public void TransitionTo(ApplicationStateArea area)
        {
            state.currentArea = area;
        }

        protected override void OnAwake()
        {
            instance.Initialize();
        }

        private void Initialize()
        {
            if (state == null)
            {
                state = new ApplicationState();
            }

            gameObject.GetOrCreateComponent(ref _screenManager);
            gameObject.GetOrCreateComponent(ref _frameStart);
            gameObject.GetOrCreateComponent(ref _frameEnd);
            gameObject.GetOrCreateComponent(ref _screenFader);
                
            if (_bootloads == null)
            {
                _bootloads = SceneBootloadDataCollection.instance;
            }

            if (state.currentArea == ApplicationStateArea.None)
            {
                state.currentArea = ApplicationStateArea.SplashScreen;
            }

            if ((state.nextArea == ApplicationStateArea.None) &&
                (state.currentArea == ApplicationStateArea.SplashScreen))
            {
                state.nextArea = ApplicationStateArea.MainMenu;
            }

            DontDestroyOnLoadSafe(gameObject);
        }

        private IEnumerator LoadScene(SceneReference sceneToLoad, Scene sceneToUnload = default(Scene), bool fadeOut = true, bool fadeIn = true)
        {
            _isSceneLoading = true;

            if (fadeOut)
            {
                _screenFader.FadeScreenOut();
                
                while (_screenFader.IsFading)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            if (sceneToUnload != default)
            {
                var unload = SceneManager.UnloadSceneAsync(sceneToUnload );
                
                while (!unload.isDone)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            
            var load = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            
            while (!load.IsDone)
            {
                yield return new WaitForEndOfFrame();
            }

            if (fadeIn)
            {
                _screenFader.FadeScreenIn();

                while (_screenFader.IsFading)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            _isSceneLoading = false;
            yield return null;
        }

        private void OnApplicationFocus(bool isFocused)
        {
            _isApplicationFocused = isFocused;
        }

        private IEnumerator ProcessBootloadData(SceneBootloadData bootloadData)
        {
            if (state.current.substate == ApplicationStates.NotLoaded)
            {
                state.current.substate = ApplicationStates.Loading;

                var subEnumerator = BootloadScenes(bootloadData);

                while (subEnumerator.MoveNext())
                {
                    yield return subEnumerator;
                }
            }
            else if (state.current.substate == ApplicationStates.Loading)
            {
                var anyPending = false;
                var totalProgress = 0f;
                var iterations = bootloadData.bootloads.Count;

                foreach (var bootload in bootloadData.bootloads)
                {
                    totalProgress += bootload.progress;

                    if (!bootload.operation.IsDone)
                    {
                        anyPending = true;
                    }
                }

                if (!anyPending)
                {
                    state.current.substate = ApplicationStates.LoadComplete;
                }
            }
        }

        private void Reset()
        {
            state = null;
            _bootloads = null;

            Initialize();
        }

        private IEnumerator Run()
        {
            Initialize();

            var currentBootloadData = _bootloads.GetByArea(state.currentArea);

            var subEnumerator = ProcessBootloadData(currentBootloadData);

            while (subEnumerator.MoveNext())
            {
                yield return subEnumerator.Current;
            }

            if (state.nextArea == ApplicationStateArea.None)
            {
                yield break;
            }

            var nextBootloadData = _bootloads.GetByArea(state.nextArea);

            subEnumerator = ProcessBootloadData(nextBootloadData);

            while (subEnumerator.MoveNext())
            {
                yield return subEnumerator.Current;
            }
        }
    }
}
