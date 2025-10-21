using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;
using Wigi.Utilities;

namespace Wigi.UI
{
    public class TouchEffectLayout : MonoBehaviour, ITouchListener
    {
        struct DecoItem
        {
            public Image deco;
            public Image square;
        }

        enum TouchState
        {
            TOUCH_NONE, TOUCH_BEGAN, TOUCH_MOVED
        }

        public int NumberDeco = 3;
        public float TimeStartEffect = 0.1f;

        public Image IconTouch;
        public Sprite[] IconSprites;

        public Image DecoTemp;

        List<DecoItem> decoArr;
        Queue<DecoItem> decoQueue;

        int sttIcon;
        float timeEffect;
        Vector2 movePos;
        TouchState touchState;
        Size screenSize;
        float _rateSize;

        // Start is called before the first frame update
        void Start()
        {
            sttIcon = 0;
            timeEffect = 0;

            decoArr = new List<DecoItem>();
            decoQueue = new Queue<DecoItem>();

            decoQueue.Enqueue(CreateDeco(DecoTemp));
            for (int i = 0; i < NumberDeco * 2; i++)
            {
                decoQueue.Enqueue(CreateDeco());
            }

            screenSize = this.GetSize();
            _rateSize = Mathf.Max(screenSize.width, screenSize.height) / 800;

            //Add Listener
            gameObject.AddComponent<TouchListener>();
        }

        private void OnDisable()
        {
            IconTouch.SetVisible(false);
            IconTouch.DOKill();
            foreach(var item in decoArr)
            {
                item.deco.SetVisible(false);
                item.deco.DOKill();
                item.square.DOKill();
            }
        }

        /*private void Update()
        {
            if(Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnPointerDown(touch);
                        break;
                    case TouchPhase.Moved:
                        OnDrag(touch);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnPointerUp(touch);
                        break;
                }
            }
        }*/

        // Update is called once per frame
        void FixedUpdate()
        {
            timeEffect += Time.fixedDeltaTime;
            if(touchState == TouchState.TOUCH_MOVED)
            {
                if(timeEffect > TimeStartEffect)
                {
                    EffectDeco(movePos);
                    timeEffect = 0;
                }
            }
        }

        DecoItem CreateDeco(Image deco = null)
        {
            DecoItem item;
            item.deco = deco != null ? deco : Instantiate(DecoTemp, this.transform);
            item.square = item.deco.GetChildByName<Image>("Square");

            item.deco.material.SetInt("_BlendSrc", (int)BlendMode.DstColor);
            item.deco.material.SetInt("_BlendDst", (int)BlendMode.One);
            item.deco.material.SetInt("_SrcBlend", (int)BlendMode.DstColor);
            item.deco.material.SetInt("_DstBlend", (int)BlendMode.One);

            item.square.material.SetInt("_BlendSrc", (int)BlendMode.DstColor);
            item.square.material.SetInt("_BlendDst", (int)BlendMode.One);
            item.square.material.SetInt("_SrcBlend", (int)BlendMode.DstColor);
            item.square.material.SetInt("_DstBlend", (int)BlendMode.One);

            decoArr.Add(item);
            return item;
        }

        void EffectTouch(Vector2 pos)
        {
            IconTouch.sprite = IconSprites[sttIcon];
            IconTouch.material.SetInt("_BlendSrc", (int) BlendMode.DstColor);
            IconTouch.material.SetInt("_BlendDst", (int)BlendMode.One);
            IconTouch.material.SetInt("_SrcBlend", (int)BlendMode.DstColor);
            IconTouch.material.SetInt("_DstBlend", (int)BlendMode.One);
            //IconTouch.setBlendFunc(cc.DST_COLOR, cc.ONE);
            IconTouch.SetWorldPosition(pos);
            IconTouch.SetVisible(true);
            IconTouch.SetAlpha(1);
            IconTouch.SetLocalScale(0);
            IconTouch.DOKill();
            IconTouch.Sequence(
                IconTouch.DOSeqDelay(0.1f),
                IconTouch.DOSeqSpawn(
                    IconTouch.DOScale(1.5f * _rateSize, 0.6f).SetEase(Ease.OutSine),
                    IconTouch.DOFadeOut(0.6f).SetEase(Ease.InSine)),
                IconTouch.DOSeqHide());

            sttIcon++;
            if (sttIcon >= IconSprites.Length) sttIcon = 0;

            for (var i = 0; i < NumberDeco; i++)
            {
                this.EffectDeco(pos);
            }
        }

        void EffectDeco(Vector2 pos)
        {
            DecoItem item;
            if (!decoQueue.TryDequeue(out item))
            {
                item = CreateDeco();
            }

            float EFF_TIME = 0.7f;
            var deco = item.deco;
            deco.DOKill();
            deco.SetVisible(true);
            deco.SetWorldPosition(pos);
            deco.SetRotationUI(Random.Range(0, 90));
            deco.SetAlpha(0);
            deco.SetLocalScale(0.2f);

            //
            float angle = Random.Range(0, Mathf.PI * 2);
            float del = (Random.Range(0, 80) + 15) * _rateSize;
            Vector3 pDelta = new Vector3(del * Mathf.Cos(angle), del * Mathf.Sin(angle), 0);
            float scale = (0.5f + Random.Range(0, 0.7f)) * _rateSize;

            deco.transform.DORotateUI(Random.Range(-0.5f, 0.5f) * 360, EFF_TIME);
            deco.transform.DOLocalMoveBy(pDelta, EFF_TIME).SetEase(Ease.OutSine);
            deco.Sequence(deco.DOScale(scale, 0.2f), deco.DOScale(scale * 0.5f, 0.5f));
            deco.Sequence(
                deco.DOFadeIn(0.4f),
                deco.DOFadeOut(0.1f),
                deco.DOFade(0.5f, 0.15f),
                deco.DOFadeOut(0.1f),
                deco.DOSeqFunc(() => { 
                    deco.SetVisible(false);
                    decoQueue.Enqueue(item);
                }));

            var square = item.square;
            square.SetAlpha(0);
            square.DOKill();
            square.Sequence(
                square.DOFadeIn(0.25f),
                square.DOFadeOut(0.1f),
                square.DOFade(0.5f, 0.2f),
                square.DOFadeOut(0.15f));
        }

        public void OnTouchBegin(TouchListener touch)
        {
            touchState = TouchState.TOUCH_BEGAN;
        }

        public void OnTouchMoved(TouchListener touch)
        {
            if (touchState == TouchState.TOUCH_BEGAN)
            {
                var delta = MathEx.ConvertDistanceToInch(touch.position - touch.beginPosition);
                if (delta > MathEx.MOVE_INCH)
                {
                    timeEffect = TimeStartEffect;
                    touchState = TouchState.TOUCH_MOVED;
                    movePos = touch.position;
                }
            }
            else if (touchState == TouchState.TOUCH_MOVED)
            {
                movePos = touch.position;
            }
        }

        public void OnTouchEnd(TouchListener touch)
        {
            touchState = TouchState.TOUCH_NONE;
            EffectTouch(touch.position);
        }
    }
}
