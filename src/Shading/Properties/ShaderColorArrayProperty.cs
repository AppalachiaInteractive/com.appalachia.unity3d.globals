using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderColorArrayProperty : ShaderProperty<ShaderColorArrayProperty, Color[]>
    {
        public ShaderColorArrayProperty()
        {
        }

        public ShaderColorArrayProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderColorArrayProperty(string propertyName, Color[] value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetColorArray(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetColorArray(PropertyId);
            }
        }
    }
}