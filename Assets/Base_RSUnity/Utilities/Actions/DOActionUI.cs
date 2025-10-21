using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

using Wigi.Utilities;

namespace Wigi.Actions
{
    public class DOActionUI : DOActionBase, IActionUI
    {
        [Space]
        [GUIColor(0.9f, 0.7f, 0.9f)]
        [PropertyOrder(10)]
        public bool backAction = false;

        //BackAction
        [GUIColor(0.9f, 0.7f, 0.9f)]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(HideAddButton = true, ShowIndexLabels = true, DraggableItems = false, ListElementLabelName = "LabelData")]
        [HorizontalGroup("ListBack", MarginRight = -50, VisibleIf = "@backAction", Order = 18)]
        [OnCollectionChanged(Before = "ChangeBackActionsList")]
        public List<ActionBase> backDatas = new List<ActionBase>();

        [FoldoutGroup("Event to first back action", Order = 20, VisibleIf = "@backAction")]
        public UnityEvent onCompleteBack;

        protected override void Initialization()
        {
            base.Initialization();
            foreach(var action in backDatas)
                action.Initialization(target);
        }

        public override void ResetDefault()
        {
            base.ResetDefault();
            foreach (var action in backDatas)
                action.ResetDefault(target);
        }

        public void DOBackAction()
        {
            if (!_isInit)
                Initialization();
            DOActionList(backDatas);
            if (tweens.Count > 0 && tweens[0] != null)
                tweens[0].onComplete += OnCompleteBack;
        }

        void OnCompleteBack()
        {
            tweens.Clear();
            onCompleteBack?.Invoke();
        }

        public void DoShowUI()
        {
            DOAction();
        }

        public void DoHideUI()
        {
            DOBackAction();
        }

        public override bool HasAction(ActionBase action)
        {
            foreach (var act in actionDatas)
                if (act == action)
                    return true;
            foreach (var act in backDatas)
                if (act == action)
                    return true;
            return false;
        }

#if UNITY_EDITOR

        [SerializeField]
        [GUIColor(0.9f, 0.7f, 0.9f)]
        [HorizontalGroup("ListBack", Width = 50, PaddingLeft = -50)]
        [ValueDropdown("ActionsClass")]
        [OnValueChanged("AddBackAction")]
        [HideLabel]
        int addBack = 0;
        int previousAddBack = -1;

        IEnumerable ActionsClass()
        {
            addBack = 0;
            return EditorUtils.GetClassForDropdown<ActionBase>("[Add Action]");
        }

        void AddBackAction()
        {
            if (addBack == previousAddBack)
            {
                addBack = 0;
                previousAddBack = -1;
                return;
            }
            if (addBack > 0)
            {
                Type actionType = EditorUtils.GetClassSelectDropdown<ActionBase>(addBack);
                if (actionType != null)
                {
                    ActionBase action = (ActionBase)Activator.CreateInstance(actionType);
                    backDatas.Add(action);
                }
                previousAddBack = addBack;
                addBack = 0;
            }
        }

        void ChangeBackActionsList(CollectionChangeInfo info, object value)
        {
            if (info.ChangeType == CollectionChangeType.RemoveIndex)
            {

            }
        }

        [HorizontalGroup("TestBack", Order = 9)]
        [Button("Reset Default")]
        void ResetBackDefault()
        {
            ResetDefault();
        }

        [HorizontalGroup("TestBack", Order = 19)]
        [Button("Reset Back")]
        void ResetBack()
        {
            foreach (var action in backDatas)
                ((ActionData)action).ResetFrom(target);
        }

        [HorizontalGroup("TestBack")]
        [Button("Test Back")]
        void TestBack()
        {
            Initialization();
            DOTween.Clear(true);
            DOTween.defaultUpdateType = UpdateType.Manual;
            DOBackAction();
            EditorApplication.update += UpdateTest;
        }
#endif

    }
}
