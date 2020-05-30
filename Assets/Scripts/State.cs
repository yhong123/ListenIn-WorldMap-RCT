using UnityEngine;
using System.Collections;

public abstract class State  {

	// Use this for initialization
    public abstract void Init();
	
	// Update is called once per frame
    public abstract void Update();

    public abstract void Exit();
}