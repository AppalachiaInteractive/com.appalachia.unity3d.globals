using System;
using System.Collections.Generic;
using Appalachia.Core.Scriptables;
using Appalachia.Globals.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Globals.Application
{
    public class
        SceneBootloadData : SelfCategorizingNamingSavingAndIdentifyingScriptableObject<SceneBootloadData>
    {
        private void Awake()
        {
            if (_scenes == null)
            {
                _scenes = new AppaList_SceneReference();
                SetDirty();
            }
        }

        [NonSerialized, ShowInInspector]
        public List<SceneBootloadProgress> bootloads;

        [SerializeField] private AppaList_SceneReference _scenes;

        [SerializeField] private bool _specifyFirst;

        [SerializeField] private bool _specifyLast;

        [ShowIf(nameof(_specifyFirst))]
        [SerializeField]
        private SceneReference _first;

        [ShowIf(nameof(_specifyLast))]
        [SerializeField]
        private SceneReference _last;

        public IEnumerable<SceneReference> GetScenesToLoad()
        {
            if (_specifyFirst && (_first != null))
            {
                yield return _first;
            }

            foreach (var scene in _scenes)
            {
                yield return scene;
            }

            if (_specifyLast && (_last != null))
            {
                yield return _last;
            }
        }
    }
}
