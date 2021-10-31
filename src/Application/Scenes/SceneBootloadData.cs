using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Scriptables;
using Appalachia.Globals.Collections;
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Appalachia.Globals.Application.Scenes
{
    public class SceneBootloadData : CategorizableAutonamedIdentifiableAppalachiaObject<SceneBootloadData>
    {
        [PropertyOrder(140)]
        [NonSerialized, ShowInInspector]
        public SceneBootloadProgress entrySceneBootloadProgress;
        
        [PropertyOrder(150)]
        [NonSerialized, ShowInInspector]
        public List<SceneBootloadProgress> bootloadProgress;

        [PropertyOrder(80)]
        public SceneReference entryScene;

        [PropertyOrder(90)]
        [SerializeField]
        private AppaList_SceneReference _scenes;

        public IEnumerable<SceneReference> GetScenesToLoad()
        {
            foreach (var scene in _scenes)
            {
                yield return scene;
            }
        }

        private void Awake()
        {
            if (bootloadProgress == null)
            {
                bootloadProgress = new List<SceneBootloadProgress>();
            }

            if (_scenes == null)
            {
                _scenes = new AppaList_SceneReference();
                SetDirty();
            }
        }

        [Button]
        [PropertyOrder(101)]
        [SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
        private void CreateScene()
        {
            var otherScene = GetScenesToLoad().FirstOrDefault();

            if (otherScene == null)
            {
                var candidateScenes = SceneBootloadDataCollection.instance.all.SelectMany(s => s._scenes);

                otherScene = candidateScenes.FirstOrDefault(s => s != null);
            }

            var otherPath = otherScene.AssetPath;
            var otherDirectory = AppaPath.GetDirectoryName(otherPath);

            var sceneName = $"{name}_{_scenes.Count}";
            var outputPath = AppaPath.Combine(otherDirectory, $"{sceneName}.unity");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            EditorSceneManager.SaveScene(scene, outputPath);
            AssetDatabaseManager.Refresh();

            EditorSceneManager.CloseScene(scene, true);

            var asset = AssetDatabaseManager.LoadAssetAtPath<UnityEditor.SceneAsset>(outputPath);
            var reference = SceneReference.CreateNew(sceneName);

            reference.SetSelection(asset);

            /*if (_specifyFirst && (_first == null))
            {
                _first = reference;
            }
            else if (_specifyLast && (_last == null))
            {
                _last = reference;
            }
            else
            {*/
            if (_scenes == null)
            {
                _scenes = new AppaList_SceneReference();
            }

            _scenes.Add(reference);
            
            SetDirty();

            //}
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(SceneBootloadData),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew();
        }
    }
}
