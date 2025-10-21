using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Wigi.Utilities
{
    public static class SpineUtils
    {
        public delegate void SpineDelegate();

        public static TrackEntry PlayAnimation(this SkeletonAnimation ske, string animation, bool isLoop = false, int track = 0)
        {
            return ske.AnimationState.SetAnimation(track, animation, isLoop);
        }

        /// <summary>
        /// Add event complete to current track of spine.
        /// </summary>
        /// <param name="ske"></param>
        /// <param name="callback"></param>
        /// <param name="track"></param>
        public static void AddCompleteAnimation(this SkeletonAnimation ske, SpineDelegate callback, int track = 0)
        {
            var trackCur = ske.AnimationState.GetCurrent(track);
            trackCur.Complete += (track) => { callback(); };
        }

        public static float GetAnimationTime(this SkeletonAnimation ske, string animation)
        {
            var anim = ske.SkeletonDataAsset.GetSkeletonData(true).FindAnimation(animation);
            if(anim != null)
                return anim.Duration;
            return 0f;
        }

        public static Tweener DoFade(this SkeletonAnimation ske, float duration, float fade = 0)
        {
            return DOTween.To(() => ske.Skeleton.A, co => { ske.Skeleton.A = co; }, fade, duration);
        }
    }
}