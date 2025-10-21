using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wigi.Utilities;


namespace Wigi.Turorial
{
    public class TutorialLayout : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float Shadow = 0.5f;
        public Button BtnBack;
        public RectTransform LayerFocus;
        public RectTransform LayerSuggest;

        Image focusShadow;
        Image rightShadow, leftShadow, topShadow, bottomShadow;
        // Start is called before the first frame update
        void Start()
        {
            focusShadow = LayerFocus.GetComponentInChildren<Image>();
            var shadow = focusShadow.transform.GetChild(0).GetComponent<Image>();
            shadow.color = Color.black;
            shadow.SetAlpha(Shadow);
            rightShadow = CreateLockShadow();
            leftShadow = CreateLockShadow();
            topShadow = CreateLockShadow();
            bottomShadow = CreateLockShadow();
        }

        Image CreateLockShadow()
        {
            var shadow = new GameObject();
            var img = shadow.AddComponent<Image>();
            img.color = Color.black;
            img.SetAlpha(Shadow);
            LayerFocus.AddChild(shadow);
            return img;
        }

        public void SetButtonTutorial()
        {

        }

        public void HideLayoutFocus()
        {
            LayerFocus.gameObject.SetActive(false);
        }

        public void SetLayoutFocus(Rect worldRect)
        {
            //Extend view rect
            float widthEx = worldRect.width * 0.25f;
            float heightEx = worldRect.height * 0.25f;
            worldRect.xMin -= widthEx / 2; worldRect.xMax += widthEx / 2;
            worldRect.yMin -= heightEx / 2; worldRect.yMax += heightEx / 2;

            //Set Focus
            LayerFocus.gameObject.SetActive(true);
            var layerSize = LayerFocus.GetSize();
            var haftSize = layerSize / 2;
            Rect localRect = worldRect.WorldToLocalRect(LayerFocus.transform);

            focusShadow.SetLocalRect(localRect);

            if(localRect.y > -haftSize.height)
            {
                bottomShadow.SetActive(true);
                bottomShadow.SetLocalRect(new Rect(-haftSize.width, -haftSize.height, layerSize.width, localRect.y + haftSize.height));
            }
            else
            {
                bottomShadow.SetActive(false);
            }

            if (localRect.yMax < haftSize.height)
            {
                topShadow.SetActive(true);
                topShadow.SetLocalRect(new Rect(-haftSize.width, localRect.yMax, layerSize.width, haftSize.height - localRect.yMax));
            }
            else
            {
                topShadow.SetActive(false);
            }

            if (localRect.x > -haftSize.width)
            {
                leftShadow.SetActive(true);
                leftShadow.SetLocalRect(new Rect(-haftSize.width, localRect.y, localRect.x + haftSize.width, localRect.height));
            }
            else
            {
                leftShadow.SetActive(false);
            }

            if (localRect.xMax < haftSize.width)
            {
                rightShadow.SetActive(true);
                rightShadow.SetLocalRect(new Rect(localRect.xMax, localRect.y, haftSize.width - localRect.xMax, localRect.height));
            }
            else
            {
                rightShadow.SetActive(false);
            }
        }

        public void OnClickFocus()
        {

        }
    }
}
