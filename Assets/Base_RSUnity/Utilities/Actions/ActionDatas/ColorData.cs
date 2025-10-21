using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Wigi.Actions
{
    [System.Serializable]
    public class ColorData : ActionData
    {
        [ShowIf("@actionType == ActionType.FromTo")]
        public Color from = Color.white;
        public Color to = Color.white;

        Color _defaultColor;

        public override Tween CreateAction(Component target)
        {
            if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                Color endTo = to;
                if (actionType == ActionType.Delta)
                {
                    endTo += sprite.color;
                }

                return sprite.DOColor(endTo, duration);
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                Color endTo = to;
                if (actionType == ActionType.Delta)
                {
                    endTo += graphic.color;
                }

                return graphic.DOColor(endTo, duration);
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                Color endTo = to;
                if (actionType == ActionType.Delta)
                {
                    endTo += renderer.color;
                }

                return renderer.DOColor(endTo, duration);
            }
            return null;
        }

        public override void ResetFrom(Component target)
        {
            if (actionType == ActionType.FromTo)
            {
                SetColor(target, from);
            }
        }
        void SetColor(Component target, Color color)
        {
            if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                sprite.color = color;
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                graphic.color = color;
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                renderer.color = color;
            }
        }

        public override void InitDefault(Component target)
        {
            if (target is SpriteRenderer)
            {
                var sprite = target as SpriteRenderer;
                _defaultColor = sprite.color;
            }
            else if (target is Graphic)
            {
                var graphic = target as Graphic;
                _defaultColor = graphic.color;
            }
            else if (target is Renderer)
            {
                var renderer = target as Graphic;
                _defaultColor = renderer.color;
            }
        }

        public override void SetDefaultInit(Component target)
        {
            SetColor(target, _defaultColor);
        }
    }
}