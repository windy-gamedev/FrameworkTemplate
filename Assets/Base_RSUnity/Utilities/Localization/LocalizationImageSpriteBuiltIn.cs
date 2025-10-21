using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ftech.Utilities
{
    public class LocalizationImageSpriteBuiltIn : LocalizationComponent
    {
        [SerializeField]
        protected Sprite[] images;
        bool RegexEscape = true;
        private Image image;
        [SerializeField]
        bool setNativeSizeWhenChange = true;
        private void Awake()
        {
            image = GetComponent<Image>();
        }

        protected override void OnChangeLanguage(string lang)
        {
            Apply();
        }

        protected override void OnEnable()
        {
            Apply();
            base.OnEnable();
        }

        void Apply()
        {
            image.sprite = images[Localization.GetCurrentLanguageIndex()];
            if (setNativeSizeWhenChange)
            {
                image.SetNativeSize();
            }
        }
    }
}