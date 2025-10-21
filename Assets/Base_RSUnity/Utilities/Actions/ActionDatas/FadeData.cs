using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [System.Serializable]
    public class FadeData : ActionData
    {
        [ShowIf("@actionType == ActionType.FromTo")]
        [Range(0, 1)]
        public float from = 0;
        [Range(0, 1)]
        public float to = 1;

        float _defaultFade = 0;

        public override void ResetFrom(Component target)
        {
            if (actionType == ActionType.FromTo)
            {
                SetAlpha(target, from);
            }
        }

        void SetAlpha(Component target, float alpha)
        {
            if (target is CanvasGroup)
            {
                var canvas = target as CanvasGroup;
                canvas.SetAlpha(alpha);
            }
            else if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                sprite.SetAlpha(alpha);
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                graphic.SetAlpha(alpha);
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                renderer.SetAlpha(alpha);
            }
        }

        public override Tween CreateAction(Component target)
        {
            if (target is CanvasGroup)
            {
                var canvas = target as CanvasGroup;
                float endTo = to;

                if (actionType == ActionType.Delta)
                {
                    endTo += canvas.alpha;
                }

                return canvas.DOFade(endTo, duration);
            }
            else if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                float endTo = to;

                if (actionType == ActionType.Delta)
                {
                    endTo += sprite.color.a;
                }

                return sprite.DOFade(endTo, duration);
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                float endTo = to;

                if (actionType == ActionType.Delta)
                {
                    endTo += graphic.color.a;
                }

                return graphic.DOFade(endTo, duration);
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                float endTo = to;

                if (actionType == ActionType.Delta)
                {
                    endTo += renderer.color.a;
                }

                return renderer.DOFade(endTo, duration);
            }

            return null;
        }

        public override void InitDefault(Component target)
        {
            if (target is CanvasGroup)
            {
                var canvas = target as CanvasGroup;
                _defaultFade = canvas.alpha;
            }
            else if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                _defaultFade = sprite.color.a;
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                _defaultFade = graphic.color.a;
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                _defaultFade = renderer.color.a;
            }
        }

        public override void SetDefaultInit(Component target)
        {
            SetAlpha(target, _defaultFade);
        }
    }
}
