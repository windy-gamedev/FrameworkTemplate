using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Wigi.Actions
{
    [System.Serializable]
    public abstract class ActionBase
    {
#if UNITY_EDITOR
        public string LabelData => "  ====== " + this.GetType().Name;
#endif

        bool _initDefault = false;
        public bool IsInitDefault => _initDefault;

        protected Component target;

        public virtual Tween Action(Component target)
        {
            this.target = target;
            var tween = SetAction(target);
            if (tween != null)
            {
                //tween.SetAutoKill(true);
                tween.SetId(target);
            }
            return tween;
        }

        protected abstract Tween SetAction(Component target);

        public virtual void Initialization(Component target)
        {
            this.target = target;
            if (!_initDefault)
            {
                InitDefault(target);
                _initDefault = true;
            }
        }

        public virtual void ResetDefault(Component target)
        {
            if (_initDefault)
                SetDefaultInit(target);
        }

        public abstract void InitDefault(Component target);
        public abstract void SetDefaultInit(Component target);

        public virtual bool ContainIs(ActionBase action)
        {
            return action == this;
        }
    }

}
