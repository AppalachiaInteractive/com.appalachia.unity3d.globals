using System;
using System.Collections;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Appalachia.Globals.Application
{
    [AlwaysInitializeOnLoad]
    public class ApplicationStateManager : SingletonMonoBehaviour<ApplicationStateManager>
    {
        static ApplicationStateManager()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => instance.Initialize();
#endif
        }
        
        [Title("Application State"), InlineProperty, HideLabel]
        public ApplicationState state;

        [Title("Bootload Data")]
        [SerializeField]
        private SceneBootloadDataCollection _bootloads;

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

        protected override void OnAwake()
        {
            instance.Initialize();
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

        public void TransitionTo(ApplicationStateArea area)
        {
            state.currentArea = area;
        }

        private void Reset()
        {
            state = null;
            _bootloads = null;

            Initialize();
        }

        private void Initialize()
        {
            if (state == null)
            {
                state = new ApplicationState();
            }

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

        private void Update()
        {
        }
    }
}
