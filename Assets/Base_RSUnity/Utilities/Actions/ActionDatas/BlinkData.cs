using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [System.Serializable]
    public class BlinkData : ActionBase
    {
        public float duration = 0.5f;
        public int frequency = 5;
        public bool timeScaleIndependent;

        bool _defaultActive = false;

        public override void InitDefault(Component target)
        {
            _defaultActive = target.GetActive();
        }

        protected override Tween SetAction(Component target)
        {
            float deltaTime = duration / (2 * frequency);
            var seq = target.Sequence(
                target.DOSeqShow(),
                target.DOSeqDelay(deltaTime), 
                target.DOSeqHide(),
                target.DOSeqDelay(deltaTime)).SetLoops(frequency);
            if (timeScaleIndependent)
                seq.SetUpdate(timeScaleIndependent);

            return seq;
        }

        public override void SetDefaultInit(Component target)
        {
            target.SetActive(_defaultActive);
        }
    }
}