using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [System.Serializable]
    public class ScaleData : ActionData
    {
        [ShowIf("@actionType == ActionType.FromTo")]
        [InlineButton("SetFrom", "Get")]
        [InlineButton("LinkFrom", "&")]
        public Vector3 from;
        [InlineButton("SetTo", "Get")]
        [InlineButton("LinkTo", "&")]
        public Vector3 to;

        Vector3 _defaultScale;

        public override void ResetFrom(Component target)
        {
            if (actionType == ActionType.FromTo)
            {
                target.transform.localScale = from;
            }
        }

        public override Tween CreateAction(Component target)
        {
            Vector3 endTo = to;
            if (actionType == ActionType.Delta)
            {
                endTo += target.transform.localScale;
            }
            return target.transform.DOScale(endTo, duration);
        }

        public override void InitDefault(Component target)
        {
            _defaultScale = target.transform.localScale;
        }

        public override void SetDefaultInit(Component target)
        {
            target.transform.localScale = _defaultScale;
        }

#if UNITY_EDITOR
        void SetFrom()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                from = target.transform.localScale;
            }
        }
        void SetTo()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                to = target.transform.localScale;
            }
        }

        void LinkFrom()
        {
            if (from.y == from.z) from = Vector3.one * from.x;
            else if (from.x == from.z) from = Vector3.one * from.y;
            else if (from.x == from.y) from = Vector3.one * from.z;
        }
        void LinkTo()
        {
            if (to.y == to.z) to = Vector3.one * to.x;
            else if (to.x == to.z) to = Vector3.one * to.y;
            else if (to.x == to.y) to = Vector3.one * to.z;
        }
#endif
    }
}
