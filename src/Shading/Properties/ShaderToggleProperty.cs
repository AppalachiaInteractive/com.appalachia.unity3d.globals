using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderToggleProperty : ShaderProperty<ShaderToggleProperty, bool>
    {
        public ShaderToggleProperty()
        {
        }

        public ShaderToggleProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderToggleProperty(string propertyName, bool value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetInt(PropertyId, Value ? 1 : 0);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetInt(PropertyId) == 1;
            }
        }
    }
}
