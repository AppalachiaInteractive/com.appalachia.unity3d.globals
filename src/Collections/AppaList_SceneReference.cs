using System;
using Appalachia.Core.Collections;
using Appalachia.Globals.Application;

namespace Appalachia.Globals.Collections
{
    [Serializable]
    public sealed class AppaList_SceneReference : AppaList<SceneReference>
    {
        public AppaList_SceneReference()
        {
        }

        public AppaList_SceneReference(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_SceneReference(AppaList<SceneReference> list) : base(list)
        {
        }

        public AppaList_SceneReference(SceneReference[] values) : base(values)
        {
        }
    }
}
