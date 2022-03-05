using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class
        ShaderComputeBufferProperty : ShaderProperty<ShaderComputeBufferProperty, ComputeBuffer>
    {
        public ShaderComputeBufferProperty()
        {
        }

        public ShaderComputeBufferProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderComputeBufferProperty(string propertyName, ComputeBuffer value, Object owner) : base(
            propertyName,
            value,
            owner
        )
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
