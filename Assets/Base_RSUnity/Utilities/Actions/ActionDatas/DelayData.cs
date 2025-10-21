using DG.Tweening;
using UnityEngine;

namespace Wigi.Actions
{
    [System.Serializable]
    public class DelayData : ActionBase
    {
        public float delay;

        public override void InitDefault(Component target)
        {
        }

        protected override Tween SetAction(Component target)
        {
            return null;
        }

        public override void SetDefaultInit(Component target)
        {
        }
    }
}
