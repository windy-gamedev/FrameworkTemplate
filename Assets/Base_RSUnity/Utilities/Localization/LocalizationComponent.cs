using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ftech.Utilities
{
    [DefaultExecutionOrder(10)]
    public abstract class LocalizationComponent : MonoBehaviour
    {
        [SerializeField]
        protected string key;

        protected abstract void OnChangeLanguage(string lang);

        protected virtual void OnEnable()
        {
            Localization.AddOnLanguageChange(OnChangeLanguage);
        }

        protected virtual void OnDisable()
        {
            Localization.RemoveOnLanguageChange(OnChangeLanguage);
        }

        public string GetContentOfKey()
        {
            return Localization.GetContentOfKey(key);
        }

        public string GetKey()
        {
            return key;
        }

        public void SetKey(string val)
        {
            key = val;
            UpdateLangage();
        }

        public void UpdateLangage()
        {
            OnChangeLanguage(Localization.GetCurrentLanguage());
        }
    }
}
