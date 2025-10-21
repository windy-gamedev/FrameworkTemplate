using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Wigi.Actions
{
    [System.Serializable]
    public class CallbackData : ActionBase
    {
        public UnityEvent callback;

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
