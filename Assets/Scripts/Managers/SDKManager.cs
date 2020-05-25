using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;

public class SDKManager : MonoBehaviour
{
    void Awake()
    {
       
       if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            FB.Init(() =>
            {
                FB.ActivateApp();
            });
        }
    }

    //     void Start()
    //     {
    //         AppsFlyer.setAppsFlyerKey("??????");
    // #if UNITY_IOS
    //         AppsFlyer.setAppID ("?????");
    //         AppsFlyer.trackAppLaunch ();
    //         AppsFlyer.setIsDebug (true);
    //         AppsFlyer.getConversionData ();
    // // #elif UNITY_ANDROID
    // //         AppsFlyer.setAppID("YOUR_ANDROID_PACKAGE_NAME_HERE");
    // //         AppsFlyer.init("YOUR_APPSFLYER_DEV_KEY", "AppsFlyerTrackerCallbacks");
    // #endif
    //     }

    public void LevelFailed()
    {
        DataSent theData = new DataSent();
        theData.level = GameManager.currentStage;
        FailedEvent(theData.level.ToString());
    }
    public void FailedEvent(string level)
    {
    /*     Debug.Log("Failed :" + level);
        var parameters = new Dictionary<string, object>();
        parameters["failed_level"] = level;
        FB.LogAppEvent(
            "FailedLevel" + level,
            null,
            parameters
        );
        */
    }




    public void LevelComplete()
    {
        DataSent theData = new DataSent();
        theData.level = GameManager.currentStage;
        LogLevelCompleteEvent(theData);
    }
    public void LogLevelCompleteEvent(DataSent data)
    {
    /*    var parameters = new Dictionary<string, object>();

        parameters[AppEventParameterName.Level] = data.level;
        // parameters["Accuracy"] = data.accuracy;

        FB.LogAppEvent(
            AppEventName.AchievedLevel,
            null,
            parameters
        );
        Debug.Log("level.....: " + data.level);
        */
    }

    public class DataSent
    {
        public int level; //which level
        // public float failed;
    }

}
