using System;
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
        
        private void Reset()
        {
            state = null;
            _bootloads = null;

            Initialize();
        }

        private void Update()
        {
            Initialize();

            var currentBootloadData = _bootloads.GetByArea(state.currentArea);

            SceneBootloader.CheckAreaLoadState(currentBootloadData, state.current);

            var nextBootloadData = _bootloads.GetByArea(state.nextArea);

            SceneBootloader.CheckAreaLoadState(nextBootloadData, state.next);
        }
    }
}
