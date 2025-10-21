using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Wigi.UI
{
    public class ClickListener : MonoBehaviour
    {
        private Collider2D Hit2DCollider;
        private Collider HitCollider;
        private RectTransform RectTransform;

        [Header("Need Collider or Rect Transform to detect click!")]
        [SerializeField]
        private UnityEvent OnClick;

        private void Awake()
        {
            Hit2DCollider = this.GetComponent<Collider2D>();

            if (Hit2DCollider == null) {
                HitCollider = this.GetComponent<Collider>();
                if (HitCollider == null)
                    RectTransform = this.GetComponent<RectTransform>();
            }
        }

        public void AddListener(UnityAction callback)
        {
            if (OnClick == null)
                OnClick = new UnityEvent();
            OnClick.AddListener(callback);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(Hit2DCollider != null)
                {
                    var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Hit2DCollider.OverlapPoint(worldPoint))
                    {
                        OnClick?.Invoke();
                    }
                    return;
                }

                if(HitCollider != null)
                {
                    var worldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (HitCollider.Raycast(worldRay, out hitInfo, 1000))
                    {
                        OnClick?.Invoke();
                    }
                    return;
                }

                if (RectTransform != null)
                {
                    var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (RectTransform.rect.Contains(RectTransform.InverseTransformPoint(worldPoint)))
                    {
                        OnClick?.Invoke();
                    }
                    return;
                }
            }
        }
    }
}
