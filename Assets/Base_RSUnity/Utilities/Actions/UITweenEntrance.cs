using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Wigi.Utilities;

namespace Wigi.Actions
{
    public class UITweenEntrance : MonoBehaviour
    {
        public UITweenEntranceStart EntranceDirection;

        private RectTransform ObjectRect;
        private Vector2 OriginalPos;
        private Vector2 EntrancePos;

        private float EntranceTime = 0.4f;
        private Ease Ease = Ease.OutQuart;

        public bool autoRun = true;
        public UnityEvent onStart, onComplete;

        private void Awake()
        {

        }

        private void OnEnable()
        {          
            if(ObjectRect == null)
            {
                ObjectRect = this.GetComponent<RectTransform>();
                OriginalPos = ObjectRect.anchoredPosition;
            }

            SetFirstPosition();

            if (autoRun)
            {
                RunTween();
            }
        }

        public void SetFirstPosition()
        {
            switch (EntranceDirection)
            {
                case UITweenEntranceStart.Top:
                    {
                        EntrancePos = new Vector2(OriginalPos.x, OriginalPos.y + (ObjectRect.GetHeight() / 2 + Camera.main.pixelHeight / 2 + 200));
                        break;
                    }
                case UITweenEntranceStart.Bottom:
                    {
                        EntrancePos = new Vector2(OriginalPos.x, OriginalPos.y - (ObjectRect.GetHeight() / 2 + Camera.main.pixelHeight / 2 + 200));
                        break;
                    }
                case UITweenEntranceStart.Left:
                    {
                        EntrancePos = new Vector2(OriginalPos.x - (ObjectRect.GetWidth() / 2 + Camera.main.pixelWidth / 2 + 200), OriginalPos.y);
                        break;
                    }
                case UITweenEntranceStart.Right:
                    {
                        EntrancePos = new Vector2(OriginalPos.x + (ObjectRect.GetWidth() / 2 + Camera.main.pixelWidth / 2 + 200), OriginalPos.y);
                        break;
                    }
            }

            ObjectRect.anchoredPosition = EntrancePos;
        }

        public void RunTween()
        {
            var action = ObjectRect.DOAnchorPos(OriginalPos, EntranceTime).SetEase(Ease);
            if (onStart != null)
                onStart?.Invoke();
            if (onComplete != null)
                action.onComplete += () => { onComplete?.Invoke(); };
        }
    }

    public enum UITweenEntranceStart
    {
        Top, Bottom, Left, Right
    }
}

