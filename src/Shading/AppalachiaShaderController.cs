using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Globals.Shading.Properties;
using Appalachia.Utility.Async;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Globals.Shading
{
    [Serializable]
    public abstract class AppalachiaShaderController<T> : AppalachiaBehaviour<T>
        where T : AppalachiaShaderController<T>
    {
        public enum ControlMode
        {
            None = 0,
            Image = 1,
            Renderer = 2,
        }

        #region Fields and Autoproperties

        public AppaEvent.Data ApplyShaderProperties;

        [SerializeField] private Shader _shader;

        [HideIf(nameof(HideRendererField))]
        public Renderer controlledRenderer;

        [PropertyRange(0, nameof(MaxMaterialIndex))]
        [HideIf(nameof(HideMaterialIndex))]
        public int materialIndex;

        [HideIf(nameof(HideImageField))]
        public Image controlledImage;

        #endregion

        public abstract string ShaderName { get; }

        protected virtual bool CanTest => false;

        public ControlMode Mode
        {
            get
            {
                using (_PRF_Mode.Auto())
                {
                    if (controlledImage != null)
                    {
                        return ControlMode.Image;
                    }

                    if (controlledRenderer != null)
                    {
                        return ControlMode.Renderer;
                    }

                    return ControlMode.None;
                }
            }
        }

        [ShowInInspector]
        public Material ControlledMaterial
        {
            get
            {
                using (_PRF_ControlledMaterial.Auto())
                {
                    Material result;

                    if (Mode == ControlMode.Image)
                    {
                        if (controlledImage == null)
                        {
                            return null;
                        }

                        result = controlledImage.materialForRendering;
                    }

                    else
                    {
                        if (controlledRenderer == null)
                        {
                            return null;
                        }

                        result = controlledRenderer.materials[materialIndex];
                    }

                    if ((result != null) && (_shader == null))
                    {
                        _shader = result.shader;
                    }

                    return result;
                }
            }
        }

        public Shader Shader
        {
            get
            {
                using (_PRF_Shader.Auto())
                {
                    if ((_shader == null) || (_shader.name != ShaderName))
                    {
                        _shader = Shader.Find(ShaderName);
                    }

                    return _shader;
                }
            }
        }

        private bool HideImageField => Mode != ControlMode.Renderer;
        private bool HideMaterialIndex => !((Mode == ControlMode.Renderer) && (controlledRenderer != null));

        private bool HideRendererField => Mode != ControlMode.Renderer;

        private int MaxMaterialIndex =>
            Mode == ControlMode.Renderer
                ? controlledRenderer == null
                    ? 0
                    : controlledRenderer.materials.Length - 1
                : 1;

        public abstract void SubscribeToPropertyChanges();

        [Button]
        public virtual void ReadFromMaterial()
        {
        }

        public void OnApplyShaderProperties()
        {
            using (_PRF_OnApplyShaderProperties.Auto())
            {
                ApplyShaderProperties.RaiseEvent();
            }
        }

        protected abstract void InitializeFields(Initializer initializer);

        [Button]
        [EnableIf(nameof(CanTest))]
        protected virtual void Test()
        {
            using (_PRF_Test.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if ((controlledRenderer == null) && (controlledImage == null))
                {
                    controlledRenderer = GetComponent<Renderer>();

                    if (controlledRenderer == null)
                    {
                        controlledImage = GetComponent<Image>();
                    }
                }

                InitializeFields(initializer);
                SubscribeToPropertyChanges();
            }
        }

        protected void UpdateShaderProperty(IShaderProperty shaderProperty)
        {
            using (_PRF_UpdateShaderProperty.Auto())
            {
                shaderProperty.ApplyToMaterial(ControlledMaterial);
            }
        }

        protected void UpdateShaderProperty(AppaEvent<IShaderProperty>.Args args)
        {
            using (_PRF_UpdateShaderProperty.Auto())
            {
                args.value.ApplyToMaterial(ControlledMaterial);
            }
        }

        protected void ValidateProperty(IShaderProperty property, string propertyName)
        {
            using (_PRF_ValidateProperty.Auto())
            {
                if (!property.HasOwner)
                {
                    property.SetOwner(this);
                }

                property.ValidatePropertyName(propertyName);

                if (!property.HasSubcribedToUpdates)
                {
                    property.HasSubcribedToUpdates = true;
                    ApplyShaderProperties.Event += () => UpdateShaderProperty(property);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ControlledMaterial =
            new ProfilerMarker(_PRF_PFX + nameof(ControlledMaterial));

        protected static readonly ProfilerMarker _PRF_InitializeFields =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeFields));

        private static readonly ProfilerMarker _PRF_Mode = new ProfilerMarker(_PRF_PFX + nameof(Mode));

        private static readonly ProfilerMarker _PRF_OnApplyShaderProperties =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyShaderProperties));

        protected static readonly ProfilerMarker _PRF_ReadFromMaterial =
            new ProfilerMarker(_PRF_PFX + nameof(ReadFromMaterial));

        private static readonly ProfilerMarker _PRF_Shader = new ProfilerMarker(_PRF_PFX + nameof(Shader));

        protected static readonly ProfilerMarker _PRF_SubscribeToPropertyChanges =
            new ProfilerMarker(_PRF_PFX + nameof(SubscribeToPropertyChanges));

        protected static readonly ProfilerMarker _PRF_Test = new ProfilerMarker(_PRF_PFX + nameof(Test));

        private static readonly ProfilerMarker _PRF_UpdateShaderProperty =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateShaderProperty));

        private static readonly ProfilerMarker _PRF_ValidateProperty =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateProperty));

        #endregion
    }
}
