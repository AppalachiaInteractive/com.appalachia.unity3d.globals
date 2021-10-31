using System;
using System.Collections;
using Appalachia.Globals.Application.State;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Appalachia.Globals.Application.Scenes
{
    public static class SceneBootloader
    {
        public static void CheckAreaLoadState(SceneBootloadData bootloadData, ApplicationSubstate state)
        {
            if (state.substate == ApplicationStates.NotLoaded)
            {
                state.substate = ApplicationStates.Loading;

                BootloadEntryScene(bootloadData);
            }
            else if (state.substate == ApplicationStates.Loading)
            {
                var operation = bootloadData.entrySceneBootloadProgress.operation;
                
                if (operation.IsDone)
                {
                    state.substate = operation.Status == AsyncOperationStatus.Succeeded
                        ? ApplicationStates.LoadComplete
                        : ApplicationStates.LoadFailed;
                }
            }
        }
        
        private static void BootloadEntryScene(SceneBootloadData bootloadData)
        {
            if (bootloadData.entryScene == null)
            {
                throw new NotSupportedException();
            }

            var assetReference = bootloadData.entryScene.sceneReference;

            var bootloadProgress = new SceneBootloadProgress
            {
                operation = assetReference.LoadSceneAsync(LoadSceneMode.Additive, false)
            };

            bootloadProgress.operation.Completed += handle => bootloadProgress.scene = handle.Result;

            bootloadData.entrySceneBootloadProgress = bootloadProgress;
        }

        private static IEnumerator BootloadAllScenes(SceneBootloadData bootloadData)
        {
            if (bootloadData.entryScene == null)
            {
                throw new NotSupportedException();
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

                bootloadData.bootloadProgress.Add(bootload);
                yield return null;
            }
        }
    }
}
