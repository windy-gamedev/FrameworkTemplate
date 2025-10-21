using UnityEngine;

//Show create file scriptable
//[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public bool isDev = true;
    public static bool IsDev => Instance.isDev;

    public int targetFrameRate = 60;
    public static int TargetFrameRate => Instance.targetFrameRate;

    public int timeSleepScreen = 180;
    public static int TimeSleepScreen => Instance.timeSleepScreen;


    public string _serverAPILink = "insight.f1dog.io/api/GameConfig/GameEnv";
    public static string SERVER_CHECK_API_LINK => Instance._serverAPILink;


    public string _gameAPILinkLive = "";
    public string _gameAPILinkDev = "";
    public static string GAME_API_LINK {
        get {
            var link = IsDev? Instance._gameAPILinkDev: Instance._gameAPILinkLive;
            return link;
        }
    }

    public string versionCode = "1.0";
    public static string VersionCode => Instance.versionCode;

    public string linkApp_Default= "";
    public string linkApp_Android = "";
    public string linkApp_Ios = "";
    public string linkApp_Web = "";

    public string linkGame_Terms = "";
    public static string LinkTerms => Instance.linkGame_Terms;
    public static string LinkApp
    {
        get
        {
#if UNITY_ANDROID
            return Instance.linkApp_Android;
#elif UNITY_IPHONE
            return Instance.linkApp_Ios;
#elif UNITY_WEBGL
            return Instance.linkApp_Web;
#endif
            return Instance.linkApp_Default;
        }
    }

    public static void InitConfig()
    {
#if UNITY_STANDALONE_WIN
#elif UNITY_EDITOR
        Application.targetFrameRate = 60;
#else
        Debug.Log("SetConfig Mobile");
        Screen.sleepTimeout = GameConfig.TimeSleepScreen;
        Application.targetFrameRate = GameConfig.TargetFrameRate;
#endif
    }

};
