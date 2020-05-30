using UnityEngine;
using System.Collections;

public class LightSwitcher : MonoBehaviour {

    public GameObject LightsRoot;
    public GameObject ActivatorsRoot;

    private LigthActivatorDestructor[] lights;
    private ButtonsActivator[] activators;

    private bool activatorsOn;
    private bool previousOn;

    // Use this for initialization
    void Start () {
        lights = LightsRoot.GetComponentsInChildren<LigthActivatorDestructor>();
        activators = ActivatorsRoot.GetComponentsInChildren<ButtonsActivator>();
        activatorsOn = true;
    }
	
	// Update is called once per frame
	void Update () {
        previousOn = activatorsOn;

        activatorsOn = false;
        if (activators != null && activators.Length != 0)
        {
            for (int i = 0; i < activators.Length; i++)
            {
                if (activators[i].Actived)
                {
                    activatorsOn = true;
                }
                else
                {
                    activatorsOn = false;
                    break;
                }
            }
        }

        if (previousOn != activatorsOn && lights != null && lights.Length != 0)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActivationState(activatorsOn);
            }
        }
    }
}
