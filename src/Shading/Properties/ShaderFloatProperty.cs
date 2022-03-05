using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderFloatProperty : ShaderProperty<ShaderFloatProperty, float>
    {
        public ShaderFloatProperty()
        {
        }

        public ShaderFloatProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderFloatProperty(string propertyName, float value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetFloat(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetFloat(PropertyId);
            }
        }
    }
}