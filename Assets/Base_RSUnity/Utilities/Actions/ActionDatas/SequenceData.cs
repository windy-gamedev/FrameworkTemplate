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
    public class SequenceData : ActionBase
    {
        public int loop = 1;

        [GUIColor(0.9f, 0.9f, 0.7f)]
        [SerializeReference]
        [HideReferenceObjectPicker]
        [HorizontalGroup("List", MarginRight = -50)]
        [ListDrawerSettings(ShowIndexLabels = true, HideAddButton = true, ListElementLabelName = "LabelData")]
        public List<ActionBase> sequences = new List<ActionBase>();

        protected override Tween SetAction(Component target)
        {
            var sequence = DOTween.Sequence();
            foreach (var seq in sequences)
            {
                if (seq is SpawnData)
                {
                    ((SpawnData)seq).ActiveSequence(sequence, target);
                }
                else
                {
                    var tween = seq.Action(target);
                    if(tween != null)
                        sequence.Append(tween);
                    else
                        AddExtendData(sequence, seq);
                }
            }

            //Set loop sequence
            if(loop != 1)
                sequence.SetLoops(loop);
            return sequence;
        }

        public static void AddExtendData(Sequence sequence, ActionBase action)
        {
            if (action is CallbackData)
            {
                sequence.AppendCallback(((CallbackData)action).callback.Invoke);
            }
            else if (action is DelayData)
            {
                sequence.AppendInterval(((DelayData)action).delay);
            }
        }

        public override void Initialization(Component target)
        {
            base.Initialization(target);
            foreach (var act in sequences)
            {
                act.Initialization(target);
            }
        }

        public override void InitDefault(Component target)
        {
            foreach (var act in sequences)
            {
                if(!act.IsInitDefault)
                    act.InitDefault(target);
            }
        }

        public override void SetDefaultInit(Component target)
        {
            foreach (var act in sequences)
            {
                act.SetDefaultInit(target);
            }
        }

        public override bool ContainIs(ActionBase action)
        {
            foreach (var act in sequences)
            {
                if (act.ContainIs(action))
                    return true;
            }
            return false;
        }

#if UNITY_EDITOR
        [SerializeField]
        [GUIColor(0.9f, 0.9f, 0.7f)]
        [HorizontalGroup("List", Width = 50, PaddingLeft = -50)]
        [ValueDropdown("ActionsClass")]
        [OnValueChanged("AddAction")]
        [HideLabel]
        int addAction = 0;
        int previousAdd = -1;

        IEnumerable ActionsClass()
        {
            addAction = 0;
            return EditorUtils.GetClassForDropdown<ActionBase>("[Add Action]");
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
                Type actionType = EditorUtils.GetClassSelectDropdown<ActionBase>(addAction);
                if (actionType != null)
                {
                    ActionBase action = (ActionBase)Activator.CreateInstance(actionType);
                    sequences.Add(action);
                }
                previousAdd = addAction;
                addAction = 0;
            }
        }
#endif
    }
}
