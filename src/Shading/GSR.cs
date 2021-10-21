#region

using System;
using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.Core.Scriptables;
using Appalachia.Core.Shading;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Shading
{
    public class GSR : SelfSavingSingletonScriptableObject<GSR>
    {
        private const string _PRF_PFX = nameof(GSR) + ".";

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_ForceReinitialze =
            new(_PRF_PFX + nameof(ForceReinitialze));

        private static readonly ProfilerMarker _PRF_InitializeShaderReferences =
            new(_PRF_PFX + nameof(InitializeShaderReferences));

        public ShaderVariantCollection shaderVariants;

        public List<Shader> leafShaders = new();
        public List<Shader> barkShaders = new();
        public List<Shader> shadowShaders = new();
        public List<Shader> grassShaders = new();
        public List<Shader> plantShaders = new();

        public Shader vspBillboardAtlas;
        public Shader vspBillboardNormals;
        public Shader treeImpostorShader;

        public Shader touchbendMovement;
        public Shader touchbendQuadRendererMask;

        public Shader touchbendQuadRendererSpatial;
        public Shader touchbendQuadGenerator;

        public Mesh touchbendQuadMesh;
        public Texture2D touchbendQuadBase;

        public Shader logShader;

        public Shader plantShader;

        public Shader debugShader;
        public Shader textureCombiner;
        public Shader textureFlipper;

        public List<Shader> otherShaders = new();

        [NonSerialized] private bool _initialized;

        protected override void OnEnable()
        {
            base.OnEnable();

            using (_PRF_OnEnable.Auto())
            {
                _initialized = false;

                for (var i = otherShaders.Count - 1; i >= 0; i--)
                {
                    if (otherShaders[i] == null)
                    {
                        otherShaders.RemoveAt(i);
                    }
                }

                for (var i = leafShaders.Count - 1; i >= 0; i--)
                {
                    if (leafShaders[i] == null)
                    {
                        leafShaders.RemoveAt(i);
                    }
                }

                for (var i = barkShaders.Count - 1; i >= 0; i--)
                {
                    if (barkShaders[i] == null)
                    {
                        barkShaders.RemoveAt(i);
                    }
                }

                for (var i = shadowShaders.Count - 1; i >= 0; i--)
                {
                    if (shadowShaders[i] == null)
                    {
                        shadowShaders.RemoveAt(i);
                    }
                }

                InitializeShaderReferences();
            }
        }

        public void ForceReinitialze()
        {
            using (_PRF_ForceReinitialze.Auto())
            {
                _initialized = false;

                InitializeShaderReferences();
            }
        }

        public void InitializeShaderReferences()
        {
            using (_PRF_InitializeShaderReferences.Auto())
            {
                if (!_initialized)
                {
                    _initialized = true;

                    foreach (var shader in leafShaders)
                    {
                        GSPL.Include(shader);
                    }

                    foreach (var shader in barkShaders)
                    {
                        GSPL.Include(shader);
                    }

                    foreach (var shader in shadowShaders)
                    {
                        GSPL.Include(shader);
                    }

                    GSPL.Include(logShader);
                    GSPL.Include(plantShader);
                    GSPL.Include(debugShader);
                    GSPL.Include(textureCombiner);
                    GSPL.Include(textureFlipper);
                    GSPL.Include(touchbendMovement);
                    GSPL.Include(touchbendQuadRendererMask);
                    GSPL.Include(touchbendQuadRendererSpatial);

                    foreach (var shader in otherShaders)
                    {
                        GSPL.Include(shader);
                    }
                }
            }
        }

#if UNITY_EDITOR
        private const string k_MenuName = APPA_MENU.BASE_AppalachiaState +
                                          APPA_MENU.ASM_AppalachiaGlobals +
                                          "Rebuild Shader Property Lookup";
        [UnityEditor.MenuItem(k_MenuName, priority = 1050)]
#endif
        public static void RebuildShaderPropertyLookup()
        {
            GSR.instance.ForceReinitialze();
        }
    }
}
