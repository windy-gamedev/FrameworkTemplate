using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [System.Serializable]
    public class RotateData : ActionData
    {
        public RotateMode rotateMode = RotateMode.Fast;
        public bool isLocal = true;
        [ShowIf("@actionType == ActionType.FromTo")]
        [InlineButton("SetFrom", "Get")]
        public Vector3 from;
        [InlineButton("SetTo", "Get")]
        public Vector3 to;

        Vector3 _defaultRotate;

        public override void ResetFrom(Component target)
        {
            if (actionType == ActionType.FromTo)
            {
                if (isLocal)
                    target.transform.localEulerAngles = from;
                else
                    target.transform.eulerAngles = from;
            }
        }

        public override Tween CreateAction(Component target)
        {
            Vector3 endTo = to;
            if (actionType == ActionType.Delta)
            {
                if (isLocal)
                    endTo += target.transform.localEulerAngles;
                else
                    endTo += target.transform.eulerAngles;
            }

            if (isLocal)
                return target.transform.DOLocalRotate(endTo, duration, rotateMode);
            return target.transform.DORotate(endTo, duration, rotateMode);
        }

        public override void InitDefault(Component target)
        {
            _defaultRotate = target.transform.localEulerAngles;
        }

        public override void SetDefaultInit(Component target)
        {
            target.transform.localEulerAngles = _defaultRotate;
        }


#if UNITY_EDITOR
        void SetFrom()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                from = isLocal ? target.transform.localEulerAngles : target.transform.eulerAngles;
            }
        }
        void SetTo()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                to = isLocal ? target.transform.localEulerAngles : target.transform.eulerAngles;
            }
        }
#endif
    }
}
