using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Ftech.Utilities
{
    public delegate string GetDirectTranslateValue();
    public class LocalizationText : LocalizationComponent
    {
        private Text text;
        private TextMeshProUGUI textTMP;
        public bool RegexEscape = true;
        public Action<string> OnChangeLanguageCallBack;

        // Use this for initialization
        void Awake()
        {
            text = GetComponent<Text>();
            textTMP = GetComponent<TextMeshProUGUI>();
        }

        protected override void OnChangeLanguage(string lang)
        {
            if (this != null)
            {
                string _text = Localization.GetContentOfKey(key, RegexEscape);
                if (_text != "")
                {
                    if (text)
                        text.text = _text;
                    else if (textTMP)
                        textTMP.text = _text;

                    OnChangeLanguageCallBack?.Invoke(_text);
                }
            }
        }
        protected override void OnEnable()
        {
            string _text = Localization.GetContentOfKey(key, RegexEscape);
            if (_text != "")
            {
                if (text)
                    text.text = _text;
                else if (textTMP)
                    textTMP.text = _text;

                OnChangeLanguageCallBack?.Invoke(_text);
            }
            base.OnEnable();
        }
    }

    public static class LocalizationTextExtension
    {
        public static void AddLocalizationComponent(this Text textObject, string keyTranslate)
        {
            LocalizationText _t = textObject.GetComponent<LocalizationText>();
            if (_t == null)
            {
                _t = textObject.gameObject.AddComponent<LocalizationText>();
            }
            _t.SetKey(keyTranslate);
        }

        public static void AddLocalizationComponent(this TextMeshProUGUI textObject, string keyTranslate)
        {
            LocalizationText _t = textObject.GetComponent<LocalizationText>();
            if (_t == null)
            {
                _t = textObject.gameObject.AddComponent<LocalizationText>();
            }
            _t.SetKey(keyTranslate);
        }

        public static string Translate(this string key, GetDirectTranslateValue directTranslateValue = null)
        {
            if (directTranslateValue != null)
            {
                return directTranslateValue();
            }
            return Localization.GetContentOfKey(key);
        }

        public static string Translate(this string key, Dictionary<string, string> translateDict, GetDirectTranslateValue directTranslateValue = null)
        {
            if (directTranslateValue != null)
            {
                return directTranslateValue();
            }
            return Localization.GetContentOfKey(key, translateDict);
        }
    }
}

