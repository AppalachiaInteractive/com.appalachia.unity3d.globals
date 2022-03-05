using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderTextureScaleProperty : ShaderProperty<ShaderTextureScaleProperty, Vector2>
    {
        public ShaderTextureScaleProperty()
        {
        }

        public ShaderTextureScaleProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderTextureScaleProperty(string propertyName, Vector2 value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetTextureScale(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetTextureScale(PropertyId);
            }
        }
    }
}