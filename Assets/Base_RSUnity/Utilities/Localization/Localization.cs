using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadWriteCsv;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.UI;
//public enum TypeLanguge
//{
//    Vietnammese,
//    English
//}
public class Localization : Singleton<Localization>
{
    public static Dictionary<string, string> languageIndex = new Dictionary<string, string>() {
        { "vn", "Việt Nam"},
        {"en", "English" }
    };
    private Dictionary<string, Dictionary<string, string>> localizationMap = new Dictionary<string, Dictionary<string, string>>();
    private List<string> languages = new List<string>();
    public int currentLanguage = 0;
    private Action<string> onLanguageChange;
    private TextAsset languageFile;

    static string languagePref = "language";

    private void Start()
    {
        //Load();
    }

    public static void SetLanguageFile(TextAsset file)
    {
        Instance.languageFile = file;
        Instance.Load();
    }

    public static void AddLanguageFile(TextAsset file)
    {
        Instance.ReadFromFile(file, false, true);
    }

    private void Load()
    {
        languages.Clear();
        localizationMap.Clear();
        if (File.Exists(Path.Combine(Application.persistentDataPath, "language.csv")))
        {
            ReloadTranslate(Path.Combine(Application.persistentDataPath, "language.csv"));
        }
        else
        {
            ReadFromFile(languageFile, true);
        }
        SetLanguage(GetSavedLanguage());
    }

    private void ReadFromFile(TextAsset file, bool header, bool update = false)
    {
        bool _header = header;
        var csv = file;

        var stream = new MemoryStream(csv.bytes);
        using (stream)
        {
            CsvFileReader reader = new CsvFileReader(stream, Encoding.UTF8);
            CsvRow row = new CsvRow();
            while (reader.ReadRow(row))
            {
                //check comment and space
                if (row.LineText == null)
                    continue;

                int column = 0;
                string key = string.Empty;
                foreach (string s in row)
                {
                    if (column == 0)
                    {
                        key = s.Trim();
                    }
                    else
                    {
                        if (_header)
                        {
                            localizationMap.Add(s, new Dictionary<string, string>());
                            languages.Add(s.Trim());
                        }
                        else
                        {
                            try
                            {
                                var lang = languages[column - 1];

                                if (update)
                                {
                                    localizationMap[lang][key] = s;
                                }
                                else
                                {
                                    localizationMap[lang].Add(key, s);
                                }
                            }
                            catch
                            {
                                //Debug.Log("key :" + key);
                            }

                        }
                    }
                    column++;
                }
                _header = false;
            }
        }
    }

    public void ReloadTranslate(string path)
    {
        languages.Clear();
        localizationMap.Clear();
        var header = true;
        byte[] csv = File.ReadAllBytes(path);
        var stream = new MemoryStream(csv);
        using (stream)
        {
            CsvFileReader reader = new CsvFileReader(stream, Encoding.UTF8);
            CsvRow row = new CsvRow();
            while (reader.ReadRow(row))
            {
                int column = 0;
                string key = string.Empty;
                foreach (string s in row)
                {
                    if (column == 0)
                    {
                        key = s.Trim();
                    }
                    else
                    {
                        if (header)
                        {
                            localizationMap.Add(s, new Dictionary<string, string>());
                            languages.Add(s.Trim());
                        }
                        else
                        {
                            var lang = languages[column - 1];
                            try
                            {
                                localizationMap[lang].Add(key, s);
                            }
                            catch
                            {
                                //Debug.Log("key :" + key);
                            }

                        }
                    }
                    column++;
                }
                header = false;
            }
        }
    }

    public static bool SetLanguage(string lang)
    {
        if (Instance == null)
        {
            Debug.Log("Localization did not init");
            return false;
        }

        int index = Instance.languages.IndexOf(lang);
        if (index < 0)
            return false;

        var lastLang = Instance.languages[(int)Instance.currentLanguage];

        if (lastLang.Equals(lang))
        {
            return true;
        }

        Instance.currentLanguage = index;
        SaveLanguage(lang);

        Instance.onLanguageChange?.Invoke(lang);
        return true;
    }

    public static bool SetLanguage(int lang)
    {
        if (Instance == null)
        {
            Debug.Log("Localization did not init");
            return false;
        }

        int index = lang;
        if (index < 0)
            return false;

        var lastIndex = Instance.currentLanguage;

        if (lastIndex.Equals(index))
        {
            return true;
        }

        Instance.currentLanguage = index;
        SaveLanguage(Instance.languages[index]);

        Instance.onLanguageChange?.Invoke(Instance.languages[index]);
        return true;
    }

    public static string To(string key, bool format = true)
    {
        return GetContentOfKey(key, format);
    }
    public static string To(string key, string code, string value, bool format = true)
    {
        return GetContentOfKey(key, format).Replace(code, value);
    }

    public static string GetContentOfKey(string key, bool format = true)
    {
        if (Instance == null)
        {
            Debug.Log("Localization did not init");
            return key;
        }

        try
        {
            if (!string.IsNullOrEmpty(key))
            {
                return format ? Regex.Unescape(Instance.localizationMap[Instance.languages[Instance.currentLanguage]][key]) :
                Instance.localizationMap[Instance.languages[Instance.currentLanguage]][key];
            }
            else
            {
                return key;
            }
        }
        catch
        {
            return key;
        }
    }

    public static string GetContentOfKey(string lang, string key, bool format = true)
    {
        if (Instance == null)
        {
            Debug.Log("Localization did not init");
            return key;
        }

        try
        {
            if (!string.IsNullOrEmpty(key))
            {
                return format ? Regex.Unescape(Instance.localizationMap[lang][key]) :
                Instance.localizationMap[lang][key];
            }
            else
            {
                return key;
            }
        }
        catch
        {
            return key;
        }
    }

    public static string GetContentOfKey(string key, Dictionary<string, string> dicValue)
    {
        string a = GetContentOfKey(key, true);

        foreach (KeyValuePair<string, string> kv in dicValue)
        {
            a = a.Replace(kv.Key, kv.Value);
        }

        return a;
    }
    public static void AddOnLanguageChange(Action<string> a)
    {
        if (Instance == null)
        {
            Debug.Log("Localization did not init");
            return;
        }

        Instance.onLanguageChange += a;
    }

    public static void RemoveOnLanguageChange(Action<string> a)
    {
        if (Instance == null)
        {
            return;
        }

        Instance.onLanguageChange -= a;
    }

    private static void SaveLanguage(string lang)
    {
        PlayerPrefs.SetString(languagePref, lang);
    }

    public static int GetSavedLanguage()
    {
        string result = PlayerPrefs.GetString(languagePref, "vn");
        return Instance.languages.IndexOf(result);
    }

    public static string GetCurrentLanguage()
    {
        return Instance.languages[Instance.currentLanguage];
    }

    public static int GetCurrentLanguageIndex()
    {
        return Instance.currentLanguage;
    }

    public static int GetLangIndex(string lang)
    {
        return Instance.languages.IndexOf(lang);
    }

    public static string[] GetLanguageList()
    {
        return Instance.languages.ToArray();
    }
}
