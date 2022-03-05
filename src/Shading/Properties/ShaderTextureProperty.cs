using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public abstract class
        ShaderTextureProperty<TShaderProperty, TValue> : ShaderProperty<TShaderProperty, TValue>
        where TShaderProperty : ShaderTextureProperty<TShaderProperty, TValue>, new()
        where TValue : Texture

    {
        protected ShaderTextureProperty()
        {
        }

        protected ShaderTextureProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        protected ShaderTextureProperty(string propertyName, TValue value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetTexture(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetTexture(PropertyId) as TValue;
            }
        }
    }
}