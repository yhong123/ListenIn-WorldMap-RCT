using UnityEngine;
using System.Collections;

public class StateExample : State
{
	#region singleton
	private static readonly StateExample instance = new StateExample();
	public static StateExample Instance
	{
		get
		{
			return instance;
		}
	}
	#endregion
	
	
	// Use this for initialization
	public override void Init()
	{

	}
	
	// Update is called once per frame
	public override void Update()
	{
		
	}
	
	public override void Exit()
	{
		
	}
}
