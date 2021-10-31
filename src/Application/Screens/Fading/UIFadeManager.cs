using System.Collections;
using Appalachia.Core.Behaviours;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Globals.Application.Screens.Fading
{
    [ExecuteAlways]
    public abstract class UIFadeManager : AppalachiaBehaviour
    {
        [Header("Scene Loading")]
        [Min(0.002f)]
        public float fadeDuration = 1.0f;

        private bool _isFading;

        public bool IsFading => _isFading;

        protected abstract void ExecuteFade(float time);

        [ButtonGroup("Fades")]
        public void FadeScreenIn()
        {
            if (_isFading)
            {
                return;
            }

            StartCoroutine(FadeScreenInCoroutine());
        }

        [ButtonGroup("Fades")]
        public void FadeScreenOut()
        {
            if (_isFading)
            {
                return;
            }

            StartCoroutine(FadeScreenOutCoroutine());
        }

        private IEnumerator FadeScreenOutCoroutine()
        {
            _isFading = true;
            
            var fadeTime = 1.0f;
            
            while (fadeTime >= 0.0f)
            {
                fadeTime -= Time.unscaledDeltaTime / fadeDuration;

                ExecuteFade(fadeTime);
                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }

        private IEnumerator FadeScreenInCoroutine()
        {
            _isFading = true;

            var fadeTime = 0.0f;
            while (fadeTime <= 1.0f)
            {
                fadeTime += Time.unscaledDeltaTime / fadeDuration;

                ExecuteFade(fadeTime);

                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }
    }
}
