using UnityEngine;
using System.Collections;

using MadLevelManager;

public class UploadManager : Singleton<UploadManager> {

    private bool backToLevelSelection = false;
    private float _currDeltaTime = 0;
    private float startUploadTime = 0;
    private float timeoutTimer = 0.0f;
    private float waitTime = 5.0f;
    private bool startTimer = false;

    public void Initialize()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //read the xml
            //Andrea: I am not sure anymore it is good to start sending query at this point.
            //StartCoroutine(DatabaseXML.Instance.ReadDatabaseXML());
        }
    }

    public void EndOfTherapyClean()
    {
        backToLevelSelection = false;
        StartCoroutine(CleanUp());
    }


    private IEnumerator CleanUp()
    {
        startUploadTime = Time.time;
        
        yield return new WaitForSeconds(1.0f);
        //Saving the jisaw pieces locally
        try
        {
            _currDeltaTime = Time.time - startUploadTime;
            Debug.Log("UploadManager: " + _currDeltaTime + " saving jigsaw pieces locally.");
            GameStateSaver.Instance.Save();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("UploadManager: " + ex.Message);
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            yield return StartCoroutine(DatabaseXML.Instance.UploadHistory2());

            //Here put all the methods with Ienumrator in order to wait them to be completed
            _currDeltaTime = Time.time - startUploadTime;
            Debug.Log("UploadManager: " + _currDeltaTime + " sending data out to the DB.");
            yield return StartCoroutine(DatabaseXML.Instance.ReadDatabaseXML());
        }
        _currDeltaTime = Time.time - startUploadTime;
        Debug.Log("UploadManager: " + _currDeltaTime + " collecting memory");

        //Collecting memory
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        
        backToLevelSelection = true;
    }

    #region Battery Level
    public float GetBatteryLevel()
    {
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                if (unityPlayer != null)
                {
                    using (AndroidJavaObject currActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        if (currActivity != null)
                        {
                            using (AndroidJavaObject intentFilter = new AndroidJavaObject("android.content.IntentFilter", new object[] { "android.intent.action.BATTERY_CHANGED" }))
                            {
                                using (AndroidJavaObject batteryIntent = currActivity.Call<AndroidJavaObject>("registerReceiver", new object[] { null, intentFilter }))
                                {
                                    int level = batteryIntent.Call<int>("getIntExtra", new object[] { "level", -1 });
                                    int scale = batteryIntent.Call<int>("getIntExtra", new object[] { "scale", -1 });

                                    // Error checking.
                                    if (level == -1 || scale == -1)
                                    {
                                        return 50f;
                                    }
                                    return ((float)level / (float)scale) * 100.0f;
                                }

                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex) { }
        return 99;
    }
    #endregion

    protected void Update()
    {
        if (backToLevelSelection)
        {

            //Waiting some extra time
            timeoutTimer += Time.deltaTime;
            if (timeoutTimer > waitTime)
            {
                _currDeltaTime = Time.time - startUploadTime;
                Debug.Log("UploadManager: " + _currDeltaTime + " returning to main screen.");
                backToLevelSelection = false;
                timeoutTimer = 0.0f;
                MadLevel.LoadLevelByName("World Map Select");
            }
        }
    }

}
