using Appalachia.Core.Behaviours;
using UnityEngine;

namespace Appalachia.Globals
{
    public class MasterObjectReference : SingletonAppalachiaBehaviour<MasterObjectReference>
    {
        public Camera mainCamera;

        public GameObject mainCharacter;
    }
}
