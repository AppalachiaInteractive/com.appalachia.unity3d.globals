using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Globals.Application.State
{
    [Serializable]
    [SmartLabelChildren]
    public class ApplicationSubstate
    {
        public ApplicationSubstate(ApplicationStateArea area)
        {
            this.area = area;
            substate = ApplicationStates.NotLoaded;
        }

        [HorizontalGroup("A"), ShowInInspector, ReadOnly]
        public readonly ApplicationStateArea area;

        [HorizontalGroup("A"), ReadOnly, GUIColor(nameof(GetSubstateColor))]
        public ApplicationStates substate;

        private Color GetSubstateColor()
        {
            switch (substate)
            {
                case ApplicationStates.NotLoaded:
                    return ColorPalette.Default.disabled.Last;
                case ApplicationStates.Loading:
                    return ColorPalette.Default.notable.Last;
                case ApplicationStates.LoadComplete:
                    return ColorPalette.Default.good.Last;
                case ApplicationStates.LoadFailed:
                    return ColorPalette.Default.bad.Last;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
