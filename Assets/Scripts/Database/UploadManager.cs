using UnityEngine;
using System.Collections;

using MadLevelManager;

public class UploadManager : Singleton<UploadManager> {

    private bool backToLevelSelection = false;
    private float timeoutTimer = 0.0f;
    private float waitTime = 5.0f;
    private bool startTimer = false;

    public void Initialize()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //read the xml
            StartCoroutine(DatabaseXML.Instance.ReadDatabaseXML());
        }
    }

    public void EndOfTherapyClean()
    {
        backToLevelSelection = false;
        StartCoroutine(CleanUp());
    }


    private IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(1.0f);
        //Saving the jisaw pieces locally
        try
        {
            Debug.Log("UploadManager: " + Time.time + " saving jigsaw pieces locally.");
            GameStateSaver.Instance.Save();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("UploadManager: " + ex.Message);
        }

        yield return StartCoroutine(DatabaseXML.Instance.UploadHistory2());


        //Here put all the methods with Ienumrator in order to wait them to be completed
        Debug.Log("UploadManager: " + Time.time + " sending data out to the DB.");
        yield return StartCoroutine(DatabaseXML.Instance.ReadDatabaseXML());

        Debug.Log("UploadManager: " + Time.time + " collecting memory");

        //Collecting memory
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        
        backToLevelSelection = true;
    }

    protected void Update()
    {
        if (backToLevelSelection)
        {

            //Waiting some extra time
            timeoutTimer += Time.deltaTime;
            if (timeoutTimer > waitTime)
            {
                Debug.Log("UploadManager: " + Time.time + " returning to main screen.");
                backToLevelSelection = false;
                timeoutTimer = 0.0f;
                MadLevel.LoadLevelByName("World Map Select");
            }
        }
    }

}
