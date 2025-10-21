using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wigi.Utilities
{
    public static class DOTweenUtils
    {
        #region Move
        public static Tween DOMove(this Component component, Vector3 position, float duration)
        {
            return ShortcutExtensions.DOMove(component.transform, position, duration).SetId(component);
        }
        public static Tween DOMoveBy(this Component component, Vector3 delta, float duration)
        {
            Vector3 pos = component.transform.position + delta;
            return ShortcutExtensions.DOMove(component.transform, pos, duration).SetId(component);
        }
        public static Tween DOLocalMove(this Component component, Vector3 position, float duration)
        {
            return ShortcutExtensions.DOLocalMove(component.transform, position, duration).SetId(component);
        }
        public static Tween DOLocalMoveBy(this Component component, Vector3 delta, float duration)
        {
            Vector3 pos = component.transform.localPosition + delta;
            return ShortcutExtensions.DOLocalMove(component.transform, pos, duration).SetId(component);
        }
        #endregion

        #region Rotate
        public static Tween DORotateUI(this Component component, float angle, float duration)
        {
            return component.transform.DORotate(new Vector3(0, 0, angle), duration).SetId(component);
        }
        #endregion

        #region Scale
        public static Tween DOScale(this Component component, float scale, float duration)
        {
            return ShortcutExtensions.DOScale(component.transform, scale, duration).SetId(component);
        }
        #endregion

        #region Fade
        public static Tween DOFadeIn(this Graphic graphic, float duration)
        {
            return graphic.DOFade(1, duration);
        }
        public static Tween DOFadeOut(this Graphic graphic, float duration)
        {
            return graphic.DOFade(0, duration);
        }
        #endregion

        #region Sequence
        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="tweens">it's Tween or TweenCallback </param>
        /// <returns></returns>
        public static Sequence Sequence(this Component component, params object[] tweens)
        {
            var sequence = DOTween.Sequence();
            sequence.Appends(tweens);
            sequence.SetId(component);
            return sequence;
        }

        public static Sequence Appends(this Sequence sequence, params object[] tweens)
        {
            foreach (var tween in tweens)
            {
                if (tween is Tween)
                {
                    sequence.Append((Tween)tween);
                }
                else if (tween is Tween[])  //Spawn
                {
                    sequence.Spawns((Tween[])tween);
                }
                else if (tween is TweenCallback)
                {
                    sequence.AppendCallback((TweenCallback)tween);
                }
                else if (tween is float)
                {
                    sequence.AppendInterval((float)tween); 
                }
            }
            return sequence;
        }
        public static Sequence Spawns(this Sequence sequence, params Tween[] tweens)
        {
            if (tweens.Length <= 0)
                return sequence;

            sequence.Append(tweens[0]);
            for (int i = 1; i < tweens.Length; i++)
            {
                sequence.Join(tweens[i]);
            }
            return sequence;
        }

        public static float DOSeqDelay(this Component component, float delay)
        {
            return delay;
        }
        public static TweenCallback DOSeqHide(this Component component)
        {
            return () => { component.gameObject.SetActive(false); };
        }
        public static TweenCallback DOSeqShow(this Component component)
        {
            return () => { component.gameObject.SetActive(true); };
        }

        public static TweenCallback DOSeqFunc(this Component component, TweenCallback func)
        {
            return func;
        }

        public static Tween[] DOSeqSpawn(this Component component, params Tween[] tweens)
        {
            return tweens;
        }
        #endregion

        #region Extra

        #endregion
    }
}
