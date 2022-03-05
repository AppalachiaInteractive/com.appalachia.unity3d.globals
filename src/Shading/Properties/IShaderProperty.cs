using Appalachia.Core.Objects.Root.Contracts;
using UnityEngine;

namespace Appalachia.Globals.Shading.Properties
{
    public interface IShaderProperty : IOwned
    {
        int PropertyId { get; }
        string Name { get; }
        void ApplyToMaterial(Material m);
        void InitializeFromMaterial(Material m);
        void ValidatePropertyName(string propertyName);
        bool HasSubcribedToUpdates { get; set; }
    }
}
