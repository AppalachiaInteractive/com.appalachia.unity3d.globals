using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderVectorArrayProperty : ShaderProperty<ShaderVectorArrayProperty, Vector4[]>
    {
        public ShaderVectorArrayProperty()
        {
        }

        public ShaderVectorArrayProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderVectorArrayProperty(string propertyName, Vector4[] value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetVectorArray(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetVectorArray(PropertyId);
            }
        }
    }
}