#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Core.Shading;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Globals.Shading
{
    public class GSR : SingletonAppalachiaObject<GSR>
    {
        

        #region Profiling And Tracing Markers

        private const string _PRF_PFX = nameof(GSR) + ".";

        private static readonly ProfilerMarker _PRF_OnEnable = new(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_ForceReinitialze =
            new(_PRF_PFX + nameof(ForceReinitialze));

        private static readonly ProfilerMarker _PRF_InitializeShaderReferences =
            new(_PRF_PFX + nameof(InitializeShaderReferences));

        #endregion

        public List<Shader> barkShaders = new();
        public List<Shader> grassShaders = new();

        public List<Shader> leafShaders = new();

        public List<Shader> otherShaders = new();
        public List<Shader> plantShaders = new();
        public List<Shader> shadowShaders = new();

        public Mesh touchbendQuadMesh;

        public Shader debugShader;

        public Shader logShader;

        public Shader plantShader;
        public Shader textureCombiner;
        public Shader textureFlipper;

        public Shader touchbendMovement;
        public Shader touchbendQuadGenerator;
        public Shader touchbendQuadRendererMask;

        public Shader touchbendQuadRendererSpatial;
        public Shader treeImpostorShader;

        public Shader vspBillboardAtlas;
        public Shader vspBillboardNormals;

        public ShaderVariantCollection shaderVariants;
        public Texture2D touchbendQuadBase;

        [NonSerialized] private bool _initialized;

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

        protected override async AppaTask OnEnable()
        {
            await base.WhenEnabled();

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

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.State.Base + "Rebuild Shader Property Lookup",
            priority = PKG.Menu.Appalachia.State.Priority
        )]
#endif
        public static void RebuildShaderPropertyLookup()
        {
            instance.ForceReinitialze();
        }
    }
}
