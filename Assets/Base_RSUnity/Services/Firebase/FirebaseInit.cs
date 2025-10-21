/*using Firebase;
using Firebase.Analytics;
using Firebase.Messaging;*/
using UnityEngine;

// Deprecated: Temporary disabled to build for ios platform

public class FirebaseInit : MonoBehaviour
{

    const string KEY_MSG_EVENT = "event";

    private void Awake()
    {
        /*Debug.Log("Init Firebase Analytics");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Debug.Log("Finish Init Firebase Analytics");
                FirebaseMessaging.GetTokenAsync().ContinueWith(task =>
                {
                    Debug.Log("Firebase token: " + task.Result);
                });

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }

            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            OnFinishInitFirebase();
        });*/
    }

    /*public void OnFinishInitFirebase()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Reg Token:" + token.Token);
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs msg)
    {
        Debug.Log("Received a new message: " + msg.Message.MessageId + " from " + msg.Message.From);

        var dataNoti = msg.Message.Data;
        foreach(var data in dataNoti)
        {
            ProcessDataMsg(data.Key, data.Value);
        }
    }

    void ProcessDataMsg(string key, string data)
    {
        switch (key){
            case KEY_MSG_EVENT:
                GameEventManager.NotifyEvent(data);
                break;
            default:
                break;
        }
    }*/
}
