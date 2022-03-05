using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class
        ShaderTexture2DProperty : ShaderTextureProperty<ShaderTexture2DProperty, Texture2D>
    {
        public ShaderTexture2DProperty()
        {
        }

        public ShaderTexture2DProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderTexture2DProperty(string propertyName, Texture2D value, Object owner) : base(propertyName, value, owner)
        {
        }
    }
}