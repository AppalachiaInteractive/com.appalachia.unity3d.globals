using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class
        ShaderGraphicsBufferProperty : ShaderProperty<ShaderGraphicsBufferProperty, GraphicsBuffer>
    {
        public ShaderGraphicsBufferProperty()
        {
        }

        public ShaderGraphicsBufferProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderGraphicsBufferProperty(string propertyName, GraphicsBuffer value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetBuffer(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
            }
        }
    }
}