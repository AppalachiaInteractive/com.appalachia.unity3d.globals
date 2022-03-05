using System;
using Appalachia.Globals.Shading.Properties.Properties;
using Appalachia.Utility.Strings;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class ShaderKeywordProperty : ShaderProperty<ShaderKeywordProperty, bool>
    {
        public ShaderKeywordProperty()
        {
        }

        public ShaderKeywordProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderKeywordProperty(string propertyName, bool value, Object owner) : base(propertyName, value, owner)
        {
        }

        public override string Name
        {
            get
            {
                var baseName = base.Name;

                return ZString.Format("{0}_ON", baseName.ToUpperInvariant());
            }
        }

        public override void ApplyToMaterial(Material m)
        {
            using (_PRF_ApplyToMaterial.Auto())
            {
                var keyword = new LocalKeyword(m.shader, Name);
                m.SetKeyword(keyword, Value);
            }
        }

        public override void InitializeFromMaterial(Material m)
        {
            using (_PRF_InitializeFromMaterial.Auto())
            {
                var keyword = new LocalKeyword(m.shader, Name);
                Value = m.IsKeywordEnabled(keyword);
            }
        }
    }
}
