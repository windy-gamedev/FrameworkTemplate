using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wigi.Utilities;

namespace Wigi.Actions
{
    /// <summary>
    /// Action Show Hide Default GUI: Scale and Fade.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class DOShowHideGUI : MonoBehaviour
    {
        [SerializeField]
        bool actionEnable = true;

        [OnValueChanged("OnSetShadow")]
        public Image Shadow;

        [SerializeField]
        [ShowIf("@Shadow != null")]
        [Range(0f, 1f)]
        float shadowAlpha;
        void OnSetShadow()
        {
            if(Shadow != null)
                shadowAlpha = Shadow.color.a;
        }

        [FoldoutGroup("Event Show Hide")]
        public UnityEvent onShow, onHide;

        CanvasGroup gui;

        // Start is called before the first frame update
        void Awake()
        {
            gui = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        private void OnEnable()
        {
            DoShowUI();
        }

        public void DoShowUI()
        {
            CreateShow();
            if (Shadow != null)
            {
                Shadow.DOKill();
                if (!Shadow.gameObject.activeSelf)
                {
                    Shadow.gameObject.SetActive(true);
                    Shadow.SetAlpha(0);
                }
                Shadow.DOFade(shadowAlpha, 0.25f).SetUpdate(true);
            }
        }

        protected virtual void CreateShow()
        {
            transform.transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
            gui.SetAlpha(0);
            gui.DOFade(1, 0.25f).SetUpdate(true).onComplete = onCompleteShow;
        }

        void onCompleteShow()
        {
            onShow?.Invoke();
        }

        public void DoHideUI()
        {
            CreateHide();
            if (Shadow != null)
            {
                Shadow.DOFade(0, 0.2f).SetUpdate(true).onComplete = () => { 
                    Shadow.gameObject.SetActive(false); 
                };
            }
        }

        protected virtual void CreateHide()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetUpdate(true);
            gui.DOFade(0, 0.2f).SetUpdate(true).onComplete = onCompleteHide;
        }

        void onCompleteHide()
        {
            this.gameObject.SetActive(false);
            onHide?.Invoke();
        }
    }
}
