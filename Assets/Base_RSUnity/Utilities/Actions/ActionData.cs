using DG.Tweening;
using System;
using UnityEngine;

namespace Wigi.Actions
{
    public abstract class ActionData : ActionBase
    {
        public enum ActionType
        {
            FromTo = 0,
            CurrentTo = 1,
            Delta = 2,
        }

        [Min(0)]
        public float duration = 0.3f;
        public int loops = 1;
        public Ease easeType = Ease.Unset;
        public ActionType actionType;

        public abstract void ResetFrom(Component target);
        public abstract Tween CreateAction(Component target);

        protected override Tween SetAction(Component target)
        {
            ResetFrom(target);
            var tween = CreateAction(target);
            //tween.OnStart(StartAction);
            if (tween != null)
            {
                if (easeType != Ease.Unset)
                    tween.SetEase(easeType);
                if (loops != 1)
                    tween.SetLoops(loops);
            }
            return tween;
        }
    }
}
