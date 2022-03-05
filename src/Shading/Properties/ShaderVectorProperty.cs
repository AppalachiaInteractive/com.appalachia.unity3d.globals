using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderVectorProperty : ShaderProperty<ShaderVectorProperty, Vector4>
    {
        public ShaderVectorProperty()
        {
        }

        public ShaderVectorProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderVectorProperty(string propertyName, Vector4 value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetVector(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetVector(PropertyId);
            }
        }
    }
}
