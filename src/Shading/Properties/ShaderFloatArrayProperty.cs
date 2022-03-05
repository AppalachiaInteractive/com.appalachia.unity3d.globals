using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderFloatArrayProperty : ShaderProperty<ShaderFloatArrayProperty, float[]>
    {
        public ShaderFloatArrayProperty()
        {
        }

        public ShaderFloatArrayProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderFloatArrayProperty(string propertyName, float[] value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetFloatArray(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetFloatArray(PropertyId);
            }
        }
    }
}