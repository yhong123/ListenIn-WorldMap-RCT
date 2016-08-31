/*
* Mad Level Manager by Mad Pixel Machine
* http://www.madpixelmachine.com
*/

using UnityEngine;
using MadLevelManager;
using System.Collections;

public class MadLevelYouAreHereScript : MonoBehaviour {

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================

    public Vector3 offset = new Vector3(0.37f, 0, 0);
    public float animationAmplitude = 0.02f;
    public float animationSpeed = 3f;
    
    private Transform lastUnlockedTransform;

    private bool initialized;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================

    // ===========================================================
    // Methods
    // ===========================================================

    void Update() {
        if (initialized) {
            float xChange = Mathf.Sin(Time.time * animationSpeed) * animationAmplitude;
            transform.position = lastUnlockedTransform.position + offset + new Vector3(xChange, 0, 0);
        }
    }

    void LateUpdate() {
        if (!initialized) {
            Initialize();
        }
    }

    private void Initialize() {

        initialized = true;
        var currentLayout = MadLevelLayout.current;
        var configuration = MadLevel.activeConfiguration;
        var group = configuration.FindGroupById(currentLayout.configurationGroup);

        var lastUnlockedLevelName = MadLevel.FindLastUnlockedLevelName(group.name);
        int lastLevelUnlocked = MadLevel.GetOrdeal(lastUnlockedLevelName, MadLevel.Type.Level);

        string[] levelNames = MadLevel.GetAllLevelNames(MadLevel.Type.Level);
        lastUnlockedLevelName = levelNames[lastLevelUnlocked - 1];

        if (lastLevelUnlocked == levelNames.Length && MadLevelProfile.GetLevelBoolean(lastUnlockedLevelName, "jigsaw_1"))
        {
            //All levels unlocked, let point to the level with less cup won
            int[] scorePoints = new int[levelNames.Length];

            for (int i = 0; i < levelNames.Length; i++)
            {
                int currScore = i;
                if (MadLevelProfile.GetLevelBoolean(levelNames[i], "jigsaw_1"))
                {
                    currScore += 10;
                }
                if (MadLevelProfile.GetLevelBoolean(levelNames[i], "jigsaw_2"))
                {
                    currScore += 100;
                }
                if (MadLevelProfile.GetLevelBoolean(levelNames[i], "jigsaw_3"))
                {
                    currScore += 1000;
                }
                scorePoints[i] = currScore;
            }

            int bestScore = 10000;
            int bestLevelIdx = 0;
            for (int i = 0; i < scorePoints.Length; i++)
            {
                if (bestScore > scorePoints[i])
                {
                    bestScore = scorePoints[i];
                    bestLevelIdx = i;
                }
            }

            lastUnlockedLevelName = levelNames[bestLevelIdx];

        }
        

        var icon = currentLayout.GetIcon(lastUnlockedLevelName);
        lastUnlockedTransform = icon.transform;
        

    }

    // ===========================================================
    // Static Methods
    // ===========================================================

    // ===========================================================
    // Inner and Anonymous Classes
    // ===========================================================

}