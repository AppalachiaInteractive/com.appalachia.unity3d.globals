using Appalachia.Core.Extensions;
using UnityEngine;

namespace Appalachia.Globals.Application.Screens.Fading
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeManager : UIFadeManager
    {
        [Header("Target")]
        public CanvasGroup menuCanvasGroup;

        private void Awake()
        {
            this.GetOrCreateComponent(ref menuCanvasGroup);
        }

        protected override void ExecuteFade(float time)
        {
            menuCanvasGroup.alpha = Mathf.Clamp01(time);
        }
    }
}
