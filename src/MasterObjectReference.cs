using Appalachia.Core.Behaviours;
using UnityEngine;

namespace Appalachia.Globals
{
    public class MasterObjectReference : SingletonMonoBehaviour<MasterObjectReference>
    {
        public Camera mainCamera;

        public GameObject mainCharacter;
    }
}
