using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

using Wigi.Utilities;

namespace Wigi.Actions
{
    public class DOActionBase : MonoBehaviour
    {
        public string LabelAction = "";
        [GUIColor(1f, 0.7f, 0.7f)]
        [SceneObjectsOnly]
        [InlineButton("Re_DetectTarget", "Find")]
        public Component target;

        public bool actionEnable = false;
        public bool timeScaleIndependent = false;

        //Actions
        [Space]
        [GUIColor(0.7f, 0.9f, 1f)]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [HorizontalGroup("List", MarginRight = -50, Order = 8)]
        [ListDrawerSettings(HideAddButton = true, ShowIndexLabels = true, DraggableItems = false, ListElementLabelName = "LabelData")]
        [OnCollectionChanged(Before = "ChangeActionsList")]
        public List<ActionBase> actionDatas = new List<ActionBase>();

        [FoldoutGroup("Event to first action", Order = 10)]
        public UnityEvent onComplete;

        protected List<Tween> tweens = new List<Tween>();

        protected bool _isInit = false;

        private void OnEnable()
        {
            if (!_isInit)
                Initialization();
            if (IsActionEnable())
                DOAction();
        }

        private void OnDisable()
        {
            ResetTweens();
        }

        public bool IsActionEnable()
        {
            return actionEnable;
        }

        protected virtual void Initialization()
        {
            foreach (var action in actionDatas)
                action.Initialization(target);
            _isInit = true;
        }

        public virtual void ResetDefault()
        {
            ResetTweens();
            foreach (var action in actionDatas)
                action.ResetDefault(target);
        }

        public virtual void DOAction()
        {
            if (!_isInit)
                Initialization();

            DOActionList(actionDatas);
            if (tweens.Count > 0 && tweens[0] != null)
                tweens[0].onComplete += OnCompleteAction;
        }

        protected void DOActionList(List<ActionBase> actions)
        {
            ResetTweens();
            foreach (var action in actions)
            {
                if (action == null)
                    continue;

                var tween = action.Action(target);
                if (tween != null)
                {
                    tweens.Add(tween);
                    if (timeScaleIndependent)
                        tween.SetUpdate(timeScaleIndependent);
                }
                else
                {
                    if (action is CallbackData)
                        ((CallbackData)action).callback.Invoke();
                }
            }
        }

        void OnCompleteAction()
        {
            tweens.Clear();
            onComplete?.Invoke();
        }

        protected void ResetTweens()
        {
            foreach(var twe in tweens)
            {
                if(twe != null && twe.active)
                    twe.Kill(false);
            }
            tweens.Clear();
        }

        protected virtual void DetectTarget()
        {
            if (target == null)
                target = GetComponent<CanvasGroup>();
            if (target == null)
                target = GetComponent<Graphic>();
            if (target == null)
                target = GetComponent<Renderer>();
            if (target == null)
                target = GetComponent<Transform>();
        }

        public virtual bool HasAction(ActionBase action)
        {
            foreach(var act in actionDatas)
                if(act.ContainIs(action))
                    return true;
            return false;
        }

#if UNITY_EDITOR
        [Space]
        [SerializeField]
        [GUIColor(0.7f, 0.9f, 1f)]
        [HorizontalGroup("List", Width = 50, PaddingLeft = -50)]
        [ValueDropdown("ActionsClass")]
        [OnValueChanged("AddAction")]
        [HideLabel]
        int addAction = 0;
        int previousAdd = -1;

        Type[] filterData = { typeof(SpawnData), typeof(DelayData) };
        IEnumerable ActionsClass()
        {
            addAction = 0;
            return EditorUtils.GetClassForDropdown<ActionBase>("[Add Action]", filterData);
        }

        void AddAction()
        {
            if(addAction == previousAdd)
            {
                addAction = 0;
                previousAdd = -1;
                return;
            }
            if (addAction > 0)
            {
                Type actionType = EditorUtils.GetClassSelectDropdown<ActionBase>(addAction, filterData);
                if (actionType != null)
                {
                    ActionBase action = (ActionBase)Activator.CreateInstance(actionType);
                    actionDatas.Add(action);
                }
                previousAdd = addAction;
                addAction = 0;
            }
        }

        void ChangeActionsList(CollectionChangeInfo info, object value)
        {
            if(info.ChangeType == CollectionChangeType.RemoveIndex)
            {
                //DestroyImmediate(actionDatas[info.Index]);
            }
        }

        //Init
        [OnInspectorInit]
        void InitInspector()
        {
            DetectTarget();
            //Set Label default
            var allDOAction = GetComponents<DOActionBase>();
            if(allDOAction.Length > 1)
            {
                int stt = 0;
                foreach(DOActionBase action in allDOAction)
                {
                    if (string.IsNullOrEmpty(action.LabelAction))
                    {
                        action.LabelAction = stt + "";
                    }
                    stt++;
                }
            }
        }

        protected virtual void Re_DetectTarget()
        {
            Component detect = null;
            if (detect == null)
                detect = target.GetComponent<CanvasGroup>();
            if (detect == null)
                detect = target.GetComponent<Graphic>();
            if (detect == null)
                detect = target.GetComponent<Renderer>();
            if (detect == null)
                detect = target.GetComponent<Transform>();
            target = detect;
        }

        [HorizontalGroup("TestAction", Order = 9)]
        [Button("Reset Default")]
        void ResetTestDefault()
        {
            ResetDefault();
        }

        [HorizontalGroup("TestAction", Order = 9)]
        [Button("Reset Action")]
        void ResetAction()
        {
            foreach (var action in actionDatas)
                ((ActionData)action).ResetFrom(target);
        }

        [HorizontalGroup("TestAction")]
        [Button("Test Action")]
        void Test()
        {
            Initialization();
            DOTween.Clear(true);
            DOTween.defaultUpdateType = UpdateType.Manual;
            DOAction();
            EditorApplication.update += UpdateTest;
        }

        protected void FinishTest()
        {
            EditorApplication.update -= UpdateTest;
            DOTween.defaultUpdateType = UpdateType.Normal;
            DOTween.Clear(true);
            oldNowTime = -1;
        }

        protected void UpdateTest()
        {
            if (tweens.Count > 0)
            {
                float dt = GetDeltaTime();
                DOTween.ManualUpdate(dt, dt);

                //check finish
                bool isComplete = true;
                foreach (var twe in tweens)
                {
                    //check error
                    if(twe == null)
                    {
                        FinishTest();
                    }

                    isComplete &= !twe.IsActive();
                }
                if (isComplete)
                {
                    FinishTest();
                }
            }
        }

        long oldNowTime = TimeUtil.Now();
        float GetDeltaTime()
        {
            if (oldNowTime < 1000)
            {
                oldNowTime = TimeUtil.Now();
                return 0;
            }
            long currTime = TimeUtil.Now();
            long dt = currTime - oldNowTime;
            oldNowTime = currTime;
            return ((float)dt) / 1000;
        }
#endif
    }
}
