#region

using System;
using System.Collections.Generic;
using Appalachia.Core.Scriptables;
using Unity.Profiling;
using UnityEngine;

#endregion

namespace Appalachia.Core.Globals.Shading
{
    public class GSR : SelfSavingSingletonScriptableObject<GSR>
    {
        private const string _PRF_PFX = nameof(GSR) + ".";
        
        public ShaderVariantCollection shaderVariants;

        public List<Shader> leafShaders = new List<Shader>();
        public List<Shader> barkShaders = new List<Shader>();
        public List<Shader> shadowShaders = new List<Shader>();
        public List<Shader> grassShaders = new List<Shader>();
        public List<Shader> plantShaders = new List<Shader>();

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

        public List<Shader> otherShaders = new List<Shader>();

        [NonSerialized] private bool _initialized;

        private static readonly ProfilerMarker _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));
        
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


        private static readonly ProfilerMarker _PRF_ForceReinitialze = new ProfilerMarker(_PRF_PFX + nameof(ForceReinitialze));
        public void ForceReinitialze()
        {
            using (_PRF_ForceReinitialze.Auto())
            {
                _initialized = false;

                InitializeShaderReferences();
            }
        }

        private static readonly ProfilerMarker _PRF_InitializeShaderReferences = new ProfilerMarker(_PRF_PFX + nameof(InitializeShaderReferences));
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
    }
}
