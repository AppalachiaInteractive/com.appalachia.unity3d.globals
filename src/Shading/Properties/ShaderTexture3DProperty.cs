using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class
        ShaderTexture3DProperty : ShaderTextureProperty<ShaderTexture3DProperty, Texture3D>
    {
        public ShaderTexture3DProperty()
        {
        }

        public ShaderTexture3DProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderTexture3DProperty(string propertyName, Texture3D value, Object owner) : base(propertyName, value, owner)
        {
        }
    }
}