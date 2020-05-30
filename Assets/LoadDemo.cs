using UnityEngine;
using System.Collections;
using MadLevelManager;

public class LoadDemo : MonoBehaviour {

    public void OpenDemoScene()
    {
        MadLevel.LoadLevelByName("Demo Level"); 
    }

}
