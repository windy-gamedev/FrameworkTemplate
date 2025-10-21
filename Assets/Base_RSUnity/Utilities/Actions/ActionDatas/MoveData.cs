using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [System.Serializable]
    public class MoveData : ActionData
    {
        public enum PositionType
        {
            Point = 0,
            InitStart = 1,
            Delta = 3,
            Transform = 2,
        }

        public bool isLocal = true;

        [ShowIf("@actionType == ActionType.Delta")]
        public bool isSaveFirstFrom = false;
        Vector3 saveFrom;

        [BoxGroup("From", VisibleIf = "@actionType == ActionType.FromTo")]
        public PositionType fromType;
        [BoxGroup("From")]
        [ShowIf("@fromType == PositionType.Point || fromType == PositionType.Delta")]
        [InlineButton("SetFrom", "Get")]
        public Vector3 from;
        [BoxGroup("From")]
        [ShowIf("@fromType == PositionType.Transform")]
        public Transform fromTrans;

        [BoxGroup("To")]
        public PositionType toType;
        [BoxGroup("To")]
        [ShowIf("@toType == PositionType.Point || toType == PositionType.Delta")]
        [InlineButton("SetTo", "Get")]
        public Vector3 to;
        [BoxGroup("To")]
        [ShowIf("@toType == PositionType.Transform")]
        public Transform toTrans;

        bool isRectTrans;
        RectTransform rectTransform;
        Vector3 _defaultPosition;

        public override void Initialization(Component target)
        {
            base.Initialization(target);
            rectTransform = target.GetComponent<RectTransform>();
            isRectTrans = rectTransform != null;

            if (isSaveFirstFrom)
            {
                saveFrom = target.transform.localPosition;
            }

            if(fromType == PositionType.InitStart)
            {
                from = isRectTrans ? rectTransform.anchoredPosition3D
                    : (isLocal ? target.transform.localPosition : target.transform.position);
            }

            if(toType == PositionType.InitStart)
            {
                to = isRectTrans ? rectTransform.anchoredPosition3D 
                    : (isLocal ? target.transform.localPosition : target.transform.position);
            }
        }

        Vector3 GetFromMove(Component target)
        {
            Vector3 resultFrom = from;

            if (fromType == PositionType.Transform)
            {
                resultFrom = isLocal ? target.transform.parent.InverseTransformPoint(fromTrans.transform.position) : fromTrans.transform.position;
            }
            else if (fromType == PositionType.Delta)
            {
                resultFrom = GetToMove(target) + resultFrom;
            }
            return resultFrom;
        }

        Vector3 GetToMove(Component target)
        {
            Vector3 resultTo = to;

            if (toType == PositionType.Transform)
            {
                resultTo = isLocal ? target.transform.parent.InverseTransformPoint(toTrans.transform.position) : toTrans.transform.position;
            }
            else if (toType == PositionType.Delta && fromType != PositionType.Delta)
            {
                resultTo = GetFromMove(target) + resultTo;
            }
            return resultTo;
        }

        public override void ResetFrom(Component target)
        {
            Vector3 firstFrom = GetFromMove(target);

            if (actionType == ActionType.FromTo)
            {
                if (isRectTrans)
                    rectTransform.anchoredPosition3D = firstFrom;
                else if (isLocal)
                    target.transform.localPosition = firstFrom;
                else
                    target.transform.position = firstFrom;
            }
            else if (actionType == ActionType.Delta)
            {
                if (isSaveFirstFrom)
                    target.transform.localPosition = saveFrom;
            }
        }

        public override Tween CreateAction(Component target)
        {
            Vector3 endTo = GetToMove(target);

            if(actionType == ActionType.Delta)
            {
                if(isRectTrans)
                    endTo = rectTransform.anchoredPosition3D + endTo;
                else if (isLocal)
                    endTo = target.transform.localPosition + endTo;
                else
                    endTo = target.transform.position + endTo;
            }

            Tween action;
            if (isRectTrans)
                action = rectTransform.DOAnchorPos3D(endTo, duration);
            else if (isLocal)
                action = target.transform.DOLocalMove(endTo, duration);
            else
                action = target.transform.DOMove(endTo, duration);
            return action;
        }

        /*public override void SetFromValue(Component target, Tween action)
        {
            Vector3 firstFrom = GetFromMove(target);

            if (actionType == ActionType.FromTo)
            {
                if (isRectTrans)
                    rectTransform.anchoredPosition3D = firstFrom;
                else if (isLocal)
                    target.transform.localPosition = firstFrom;
                else
                    target.transform.position = firstFrom;
            }
            else if (actionType == ActionType.Delta)
            {
                if (isSaveFirstFrom)
                    target.transform.localPosition = saveFrom;
            }

            if (action is TweenerCore<Vector3, Vector3, VectorOptions>)
            {
                ((TweenerCore<Vector3, Vector3, VectorOptions>)action).ChangeStartValue(GetFromMove(target));
            }
        }*/

        public override void InitDefault(Component target)
        {
            _defaultPosition = target.transform.localPosition;
        }

        public override void SetDefaultInit(Component target)
        {
            target.transform.localPosition = _defaultPosition;
        }

#if UNITY_EDITOR
        void SetFrom()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                from = isLocal ? target.transform.localPosition : target.transform.position;
            }
        }
        void SetTo()
        {
            var target = DOActionUtils.GetTargetDOAction(EditorUtils.GetSelectObjectInspector(), this);
            if (target != null)
            {
                to = isLocal ? target.transform.localPosition : target.transform.position;
            }
        }

        private void OnValidate()
        {
            if(fromType == PositionType.Delta && toType == PositionType.Delta)
            {
                toType = PositionType.Point;
            }
        }
#endif
    }
}
