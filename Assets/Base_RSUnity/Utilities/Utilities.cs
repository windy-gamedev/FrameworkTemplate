using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace Wigi.Utilities
{
    //StringUtil
    public static class StringUtil
    {
        static CultureInfo DEAFAULT_CULTURE = CultureInfo.CreateSpecificCulture("en-US");

        #region Color
        public static string ColorToHex(this Color color)
        {
            Color32 color32 = (Color32)(color);
            byte[] arr = { color32.r, color32.g, color32.b, color32.a };
            return BitConverter.ToString(arr);
        }
        public static Color HexToColor(this string colorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(colorStr, out color);
            if (color == null)
                color = Color.white;
            return color;
        }
        #endregion

        #region Enum
        public static T StringToEnum<T>(string value) where T : System.Enum
        {
            return (T)System.Enum.Parse(typeof(T), value);
        }

        public static T IntToEnum<T>(int value) where T : System.Enum
        {
            return (T)System.Enum.ToObject(typeof(T), value);
        }

        public static T ToEnum<T>(this int value) where T : Enum
        {
            return IntToEnum<T>(value);
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            return StringToEnum<T>(value);
        }
        #endregion

        #region Number
        public static string Decimal2Digit(double value)
        {
            return value.ToString("n2", DEAFAULT_CULTURE);
        }

        public static string DecimalNumberFormater(this double value, int decimalCount = 2)
        {
            return value.ToString("#,#0." + new string('#', decimalCount), DEAFAULT_CULTURE);
        }
        public static string DecimalNumberFormater(this decimal value, int decimalCount = 2)
        {
            return value.ToString("#,#0." + new string('#', decimalCount), DEAFAULT_CULTURE);
        }

        public static string LongNumberFormater(long value)
        {
            return value.ToString("n0", DEAFAULT_CULTURE);
        }
        public static string PointNumber(this int value)
        {
            return LongNumberFormater(value);
        }

        public static string PointNumber(this long value)
        {
            return LongNumberFormater(value);
        }

        public static string PointNumber(this double value, int decimalCount = 2)
        {
            return DecimalNumberFormater(value, decimalCount);
        }
        public static string PointNumber(this float value, int decimalCount = 2)
        {
            return DecimalNumberFormater(value, decimalCount);
        }
        public static string PointNumber(this decimal value, int decimalCount = 2)
        {
            return DecimalNumberFormater(value, decimalCount);
        }
        public static string FormatPercent(this float value, int decimalCount = 0)
        {
            string format = "{0:P" + decimalCount.ToString() + "}";
            return string.Format(format, value);
        }
        public static string FormatZero(this int value, int numberCount = 2)
        {
            return value.ToString("D" + numberCount);
        }

        public static string Format(this int value)
        {
            return LongNumberFormater(value);
        }

        public static string Format(this long value)
        {
            return LongNumberFormater(value);
        }

        public static string Format(this double value)
        {
            return Decimal2Digit(value);
        }

        public static string Format(this float value)
        {
            return Decimal2Digit(value);
        }

        public static int GetIntFromString(string str)
        {
            string num = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                    num += str[i];
            }

            if (num.Length > 0)
                return int.Parse(num);
            return 0;
        }
        #endregion

        #region Name
        public static string LimitName(string value, int characterLimit = 12, string replaceCharacter = "..")
        {
            if (value.Length > characterLimit - replaceCharacter.Length)
            {
                value = value.Substring(0, characterLimit - replaceCharacter.Length);
                value += replaceCharacter;
            }
            return value;
        }

        public static string Format(this string value, int characterLimit = 12, string replaceCharacter = "..")
        {
            return LimitName(value, characterLimit, replaceCharacter);
        }
        #endregion

        #region Time
        public static string ToTimeString(this long value)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(value);
            return ts.ToString(@"dd\:hh\:mm\:ss");
        }
        #endregion

        #region Text
        public static string AddErrorCode(this string s, string errorCode)
        {
            return s + " <" + errorCode + ">";
        }

        public static void CopyToClipboard(this string s)
        {
            TextEditor te = new TextEditor();
            te.text = s;
            te.SelectAll();
            te.Copy();
        }

        public static byte[] Compress(this string text)
        {
            var bytes = Encoding.Unicode.GetBytes(text);
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    gs.Write(bytes, 0, bytes.Length);
                }
                return mso.ToArray();
            }
        }

        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "?", "ã", "?", "â", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?", "?",
    "?",
    "é","è","?","?","?","ê","?","?","?","?","?",
    "í","ì","?","?","?",
    "ó","ò","?","õ","?","ô","?","?","?","?","?","?","?","?","?","?","?",
    "ú","ù","?","?","?","?","?","?","?","?","?",
    "ý","?","?","?","?",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        #endregion
    }

    public static class TimeUtil
    {

        #region Game Time
        /// <summary>
        /// Get second now Time from Start Game
        /// </summary>
        /// <returns></returns>
        public static float NowSecondGame()
        {
            return Time.realtimeSinceStartup;
        }
        /// <summary>
        /// Get Millisecond now Time from Start Game
        /// </summary>
        /// <returns></returns>
        public static long NowGame()
        {
            return (long)NowSecondGame() * 1000;
        }
        #endregion
        #region Epoch Time
        public static long DEFAULT_TIME_STAMP_TICK = 621355968000000000; //new DateTime(1970, 1, 1)
        /// <summary>
        /// Get Time Stamp Millisecond now. (raw new DateTime(1970, 1, 1))
        /// </summary>
        /// <returns></returns>
        public static long Now()
        {
            return (DateTime.Now.Ticks - DEFAULT_TIME_STAMP_TICK) / TimeSpan.TicksPerMillisecond;
        }
        /// <summary>
        /// Get Time Stamp second now.
        /// </summary>
        /// <returns></returns>
        public static double NowSecond()
        {
            return (double)Now() / 1000;
        }
        public static float NowDeltaSecond(double pointTime)
        {
            return (float)(NowSecond() - pointTime);
        }

        public static long DateToTimestamp(this DateTime dateTime)
        {
            return (dateTime.Ticks - DEFAULT_TIME_STAMP_TICK) / TimeSpan.TicksPerMillisecond;
        }

        public static DateTime TimestampToDate(long timestamp, DateTimeKind kind = DateTimeKind.Utc)
        {
            return new DateTime(DEFAULT_TIME_STAMP_TICK + (timestamp * TimeSpan.TicksPerSecond), kind);
        }
        #endregion

        public static DateTime TimeStringToDate(string strTime)
        {
            try
            {
                return DateTime.ParseExact(strTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            } catch (Exception e) { }

            try
            {
                return DateTime.ParseExact(strTime, "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
            }
            catch (Exception e) { }

            try
            {
                return DateTime.Parse(strTime);
            }
            catch (Exception e) { }

            return DateTime.Now;
        }
        /// <summary>
        /// Get get String Time from milli second
        /// </summary>
        /// <param name="value">milli second</param>
        /// <returns></returns>
        public static string ToTimeString(this long value)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(value);
            return ts.ToString(@"dd\:hh\:mm\:ss");
        }
        /// <summary>
        /// Get get String Time from  second
        /// </summary>
        /// <param name="value">milli second</param>
        /// <returns></returns>
        public static string ToTimeString(this double value)
        {

            int min = (int)value / 60;
            int second = (int)value % 60;
            int milSecond = (int)(value % 1f * 100);

            string strTime = min.ToString("D2") + ":" + second.ToString("D2") + ":" + milSecond.ToString("D2");
            return strTime;
        }

        #region Time Log
        static long _timeStartLog = 0;
        static long _stopWatchLog = 0;

        public static void StartLog()
        {
            _timeStartLog = Now();
            _stopWatchLog = _timeStartLog;
        }

        public static void TimeLog(string msg)
        {
            var now = Now();
            Debug.Log(msg + ": time [ " + (now - _stopWatchLog) + "ms - " + (now - _timeStartLog) + "ms ]");
            _stopWatchLog = now;
        }
        #endregion
    }

    public static class LayerUtil
    {
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask.value == (layerMask.value | (1 << layer));
        }

        public static void AddLayer(this ref LayerMask layerMask, int layer)
        {
            layerMask = layerMask.value | (1 << layer);
        }

        public static void MinusLayer(this ref LayerMask layerMask, int layer)
        {

            layerMask = layerMask.value & ~(1 << layer);
        }

        public static LayerMask LayerToMask(int layer)
        {
            var mask = default(LayerMask);
            mask.AddLayer(layer);
            return mask;
        }
    }

    public static class EnumUtil
    {
        public static T Next<T>(this T src) where T : System.Enum
        {
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static T Previous<T>(this T src) where T : System.Enum
        {
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) - 1;
            return (j < 0) ? Arr[0] : Arr[j];
        }
    }


}
