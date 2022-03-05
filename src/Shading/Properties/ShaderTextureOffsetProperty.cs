using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderTextureOffsetProperty : ShaderProperty<ShaderTextureOffsetProperty, Vector2>
    {
        public ShaderTextureOffsetProperty()
        {
        }

        public ShaderTextureOffsetProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderTextureOffsetProperty(string propertyName, Vector2 value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetTextureOffset(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetTextureOffset(PropertyId);
            }
        }
    }
}