using System;
using Appalachia.Globals.Shading.Properties.Properties;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderMatrixProperty : ShaderProperty<ShaderMatrixProperty, Matrix4x4>
    {
        public ShaderMatrixProperty()
        {
        }

        public ShaderMatrixProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderMatrixProperty(string propertyName, Matrix4x4 value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                m.SetMatrix(PropertyId, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                Value = m.GetMatrix(PropertyId);
            }
        }
    }
}