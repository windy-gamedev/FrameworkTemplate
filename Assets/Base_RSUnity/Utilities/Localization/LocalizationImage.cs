using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ftech.Utilities
{
    public class LocalizationImage : LocalizationComponent
    {
        private Image image;
        bool RegexEscape = true;
        static TranslateImage[] images;

        public static void SetImage(TranslateImage[] sprites)
        {
            images = sprites;
        }
        private void Awake()
        {
            image = GetComponent<Image>();
        }
        //protected override void OnChangeLanguage(string lang)
        //{
        //    string src = Localization.Get(key, RegexEscape);
        //    image.sprite = sprites[Localization.GetLangIndex(lang)];
        //    image.SetNativeSize();
        //}

        //protected override void OnEnable()
        //{
        //    string lang = Localization.GetCurrentLangue();
        //    image.sprite = sprites[Localization.GetLangIndex(lang)];
        //    image.SetNativeSize();
        //    base.OnEnable();
        //}

        [SerializeField]
        int imageIndex;

        public void SetKey(int val)
        {
            imageIndex = val;
            UpdateLangage();
        }

        protected override void OnChangeLanguage(string lang)
        {
            string src = Localization.GetContentOfKey(key, RegexEscape);
            try
            {
                image.sprite = images[Localization.GetCurrentLanguageIndex()].sprites[imageIndex];
                image.SetNativeSize();
            }
            catch { }
        }

        protected override void OnEnable()
        {
            try
            {
                string src = Localization.GetContentOfKey(key, RegexEscape);
                image.sprite = images[Localization.GetCurrentLanguageIndex()].sprites[imageIndex];
                image.SetNativeSize();
            }
            catch
            {
            }
            base.OnEnable();
        }
    }


    [Serializable]
    public struct TranslateImage
    {
        public Sprite[] sprites;
    }
}
