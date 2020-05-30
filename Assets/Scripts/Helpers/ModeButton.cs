using UnityEngine;
using System.Collections;

public class ModeButton : MonoBehaviour {

    //a script for selecting the mode of play, sends message up to the menu controller
    void OnMouseDown()
    {
        if(this.tag == "automatic") gameObject.SendMessageUpwards("Clicked","automatic");
        else if(this.tag == "manual") gameObject.SendMessageUpwards("Clicked", "manual");
    }

}
