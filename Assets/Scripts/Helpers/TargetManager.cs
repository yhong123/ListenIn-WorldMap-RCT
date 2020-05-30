using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TargetManager : MonoBehaviour {

    public enum Flavor { Red = 0, Brown = 1, Green = 2, Blue = 3, Black = 4, None = 5};

    private DuckRotation[] ducks;

    /// <summary>
    /// There are 5 covers,  each position is matched with a flavor, the short keep tracks of how many ducks with that flavor are rotated
    /// </summary>
    private short[] flavorDucks =  new short[5];
    /// <summary>
    /// Tracks covers curr flavor (each cover must have one flavor)
    /// </summary>
    public Flavor[] flavors = new Flavor[5];

	public List<GameObject> covers;
	public List<bool> previousOpened;
	public List<bool> currOpened;

    private List<List<LeverActivator>> totalLevers;

	// Use this for initialization
	void Start () {
        
        ducks = gameObject.GetComponentsInChildren<DuckRotation>();
		totalLevers = new List<List<LeverActivator>>();
		previousOpened = new List<bool>();
		currOpened = new List<bool>();

        //Adding dynamic configurations for the covers
        covers = new List<GameObject>();
        covers.AddRange(GameObject.FindGameObjectsWithTag("Cover"));

		for (int i = 0; i < covers.Count; i++) {

            flavors[i] = covers[i].GetComponent<CoverFlavor>().currentFlavor;

			totalLevers.Add(new List<LeverActivator>());
			previousOpened.Add(false);
			currOpened.Add(false);
			LeverActivator[] levers = covers[i].GetComponentsInChildren<LeverActivator>();
			if(levers.Length == 0) Debug.LogWarning("Activators not found");
			for (int j = 0; j < levers.Length; j++) {
				totalLevers[i].Add(levers[j]);
			}
		}
	}

	public void PlaySound(bool active)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = active ? "Levels/FancyIsland/FlipperOpen" : "Levels/FancyIsland/FlipperClose" ;
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx, aci);
	}

	void LeverState(int num, bool  opened)
	{
		previousOpened[num] = currOpened[num];
		currOpened[num] = opened;

		if(previousOpened[num] != currOpened[num])
		{
			PlaySound(currOpened[num]);
		}

		foreach (LeverActivator act in totalLevers[num]) 
		{
			act.Open = opened;		
		}

	}

    void OpenActivatorsFlavor()
    {
        short currPerc;
        short counter = 0;
        List<short> currlevers = new List<short>();

        for (int i = 0; i < flavorDucks.Length; i++)
        {
            counter = 0;
            currlevers.Clear();
            // 0 is red, 1 is brown and so on
            currPerc = flavorDucks[i];
            //Getting index from coverflavors
            for (short z = 0; z < flavors.Length; z++)
            {
                if (flavors[z] == (TargetManager.Flavor)i)
                {
                    currlevers.Add(z);
                }
            }

            for (int x = 0; x < currlevers.Count; x++)
            {
                if (currPerc > 0)
                {
                    LeverState(currlevers[x], true);
                    currPerc--;
                }
                else
                {
                    LeverState(currlevers[x], false);
                }
            }
        } 
    }

	void OpenActivators(float per)
	{
		if(per > 0.10f && per < 0.25f)
		{
			LeverState(0, true);
			LeverState(1, false);
			LeverState(2, false);
			LeverState(3, false);
			LeverState(4, false);
		}
		else if (per >= 0.25f && per < 0.40f)
		{
			LeverState(0, true);
			LeverState(1, true);
			LeverState(2, false);
			LeverState(3, false);
			LeverState(4, false);
		}
		else if (per >= 0.40f && per < 0.55f)
		{
			LeverState(0, true);
			LeverState(1, true);
			LeverState(2, true);
			LeverState(3, false);
			LeverState(4, false);
		}
		else if (per >= 0.55f && per < 0.65f)
		{
			LeverState(0, true);
			LeverState(1, true);
			LeverState(2, true);
			LeverState(3, true);
			LeverState(4, false);
		}
		else if (per >= 0.65f)
		{
			LeverState(0, true);
			LeverState(1, true);
			LeverState(2, true);
			LeverState(3, true);
			LeverState(4, true);
		}
		else
		{
			LeverState(0, false);
			LeverState(1, false);
			LeverState(2, false);
			LeverState(3, false);
			LeverState(4, false);
		}
	}

	void CheckActivation()
	{
        //Resetting openings
		for (int i = 0; i < 5; i++){
            flavorDucks[i] = 0;
        }

		for (int i = 0; i < ducks.Length; i++) {
			if (ducks[i].finishedActivation) {
                Flavor currFlavor = ducks[i].duckFlavor;
                if(currFlavor != Flavor.None)
                    flavorDucks[(int)currFlavor] += 1;
			} 
		}

		//return (float)count/ducks.Length;
	}

	// Update is called once per frame
	void Update() {
		CheckActivation();
        OpenActivatorsFlavor();
        //OpenActivators(percentage);
    }
}
