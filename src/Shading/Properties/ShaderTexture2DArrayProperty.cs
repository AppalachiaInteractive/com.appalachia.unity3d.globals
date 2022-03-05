using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Globals.Shading.Properties
{
    [Serializable]
    public sealed class
        ShaderTexture2DArrayProperty : ShaderTextureProperty<ShaderTexture2DArrayProperty, Texture2DArray>
    {
        public ShaderTexture2DArrayProperty()
        {
        }

        public ShaderTexture2DArrayProperty(string propertyName, Object owner) : base(propertyName, owner)
        {
        }

        public ShaderTexture2DArrayProperty(string propertyName, Texture2DArray value, Object owner) : base(propertyName, value, owner)
        {
        }
    }
}