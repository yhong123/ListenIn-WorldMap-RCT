using UnityEngine;
using System.Collections;

public class PatientAddedPanel : MonoBehaviour {

	public void SettoFalse()
    {
        GetComponent<Animator>().SetBool("Start", false);
    }
}
