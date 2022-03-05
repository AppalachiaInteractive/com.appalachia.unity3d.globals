using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderIntProperty : ShaderProperty<ShaderIntProperty, int>
    {
        public ShaderIntProperty()
        {
        }

        public ShaderIntProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderIntProperty(string propertyName, int value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetInt(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetInt(PropertyId);
            }
        }
    }
}