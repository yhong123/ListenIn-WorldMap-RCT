using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(SplineController))]
public class CreateSplineControllerTransform : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		SplineController script =  (SplineController)target;

		if(GUILayout.Button("AddSplineToController"))
		{
			script.AddSplineToRootObject();
		}
	}
}
#endif