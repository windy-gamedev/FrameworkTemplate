using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wigi.Utilities;

namespace Wigi.Actions
{
    [Serializable]
    public class SpawnData : ActionBase
    {
        [GUIColor(0.7f, 0.9f, 0.7f)]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [HorizontalGroup("List", MarginRight = -50)]
        [ListDrawerSettings(DraggableItems = false, Expanded = true, ShowIndexLabels = true, HideAddButton = true, ListElementLabelName = "LabelData")]
        public List<ActionBase> spawns = new List<ActionBase>();
        protected override Tween SetAction(Component target)
        {
            return null;
        }

        public void ActiveSequence(Sequence sequence, Component target)
        {
            bool isFirst = true;
            foreach (var action in spawns)
            {
                var tween = action.Action(target);
                if (tween != null)
                {
                    if (isFirst)
                    {
                        sequence.Append(tween);
                        isFirst = false;
                    }
                    else
                    {
                        sequence = sequence.Join(tween);
                    }
                }
            }
        }

        public override void Initialization(Component target)
        {
            base.Initialization(target);
            foreach (var act in spawns)
            {
                act.Initialization(target);
            }
        }

        public override void InitDefault(Component target)
        {
            foreach (var act in spawns)
            {
                if (!act.IsInitDefault)
                    act.InitDefault(target);
            }
        }

        public override void SetDefaultInit(Component target)
        {
            foreach (var act in spawns)
            {
                act.SetDefaultInit(target);
            }
        }

        public override bool ContainIs(ActionBase action)
        {
            foreach (var act in spawns)
            {
                if (act.ContainIs(action))
                    return true;
            }
            return false;
        }

#if UNITY_EDITOR
        [SerializeField]
        [GUIColor(0.7f, 0.9f, 0.7f)]
        [HorizontalGroup("List", Width = 50, PaddingLeft = -50)]
        [ValueDropdown("ActionsClass")]
        [OnValueChanged("AddAction")]
        [HideLabel]
        int addAction = 0;
        int previousAdd = -1;

        Type[] filterData = { typeof(SpawnData), typeof(CallbackData), typeof(DelayData) };
        IEnumerable ActionsClass()
        {
            addAction = 0;
            return EditorUtils.GetClassForDropdown<ActionBase>("[Add Action]", filterData);
        }

        void AddAction()
        {
            if (addAction == previousAdd)
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
                    spawns.Add(action);
                }
                previousAdd = addAction;
                addAction = 0;
            }
        }
#endif
    }
}
