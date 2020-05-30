using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextFix : MonoBehaviour {

	// quick fix for 3d text to appear above other layers
	void Start () {
        GetComponent<Renderer>().sortingLayerID = 0;
        GetComponent<Renderer>().sortingOrder = 5;
	}

}
