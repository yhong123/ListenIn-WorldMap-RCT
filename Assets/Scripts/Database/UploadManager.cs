using UnityEngine;
using System.Collections;
using System;

using MadLevelManager;

public class UploadManager : Singleton<UploadManager> {

    public float currentBatteryLevel = 0.0f;
    private bool backToLevelSelection = false;
    private float _currDeltaTime = 0;
    private float startUploadTime = 0;
    private float timeoutTimer = 0.0f;
    private float waitTime = 3.0f;
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
    #region THERAPY
    public IEnumerator EndOfTherapyClean(int correctAnswer = 0, string fileToDelete = "")
    {
        backToLevelSelection = false;
        yield return StartCoroutine(CleanUp(correctAnswer, fileToDelete));
    }
    private IEnumerator CleanUp(int correctAnswer, string fileToDelete = "")
    {
        startUploadTime = Time.time;
        //AndreaLIRO: adding the LIRO therapy update
        yield return StartCoroutine(TherapyLIROManager.Instance.AdvanceCurrentBlockInSection(fileToDelete));
        Debug.Log("UploadManager: " + startUploadTime + " saving the LIRO therapy");
    }
    #endregion THERAPY

    #region ACT
    public IEnumerator EndOfACTClean(int correctAnswer = 0)
    {
        backToLevelSelection = false;
        yield return StartCoroutine(CleanUpACT(correctAnswer));
    }
    private IEnumerator CleanUpACT(int correctAnswer)
    {
        startUploadTime = Time.time;
        Debug.Log("UploadManager: " + startUploadTime + " saving ACT");
        //AndreaLIRO: adding the LIRO therapy update
        yield return StartCoroutine(TherapyLIROManager.Instance.AdvanceCurrentBlockInSection());
        yield return StartCoroutine(TherapyLIROManager.Instance.UpdateACTScore(correctAnswer));

        _currDeltaTime = Time.time - startUploadTime;
        Debug.Log("UploadManager: " + _currDeltaTime + " collecting memory");

        CollectAndBackToMainHub();
    }
    #endregion ACT

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
                                        currentBatteryLevel = 0.0f;
                                        return currentBatteryLevel;
                                    }
                                    currentBatteryLevel = ((float)level / (float)scale) * 100.0f;
                                    return currentBatteryLevel;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex) { }
        currentBatteryLevel = 101.0f;
        return currentBatteryLevel;
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
                MadLevel.LoadLevelByName("MainHUB");
            }
        }
    }

    public void CollectAndBackToMainHub()
    {
        CollectMemory();
        backToLevelSelection = true;
    }

    private void CollectMemory()
    {
        //Collecting memory
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
    }
}
