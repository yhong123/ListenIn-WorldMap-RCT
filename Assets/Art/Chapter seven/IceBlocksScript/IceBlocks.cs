using UnityEngine;
using System.Collections;

public class IceBlocks : MonoBehaviour {
	//public GameObject movingIce = null; 

	// Use this for initialization
	public Vector2 pointB;
	
	IEnumerator Start()
	{
		Vector2 pointA = transform.position;
		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, 1.0f));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, 1.0f));
		}
	}
	
	IEnumerator MoveObject(Transform thisTransform, Vector2 startPos, Vector2 endPos, float time)
	{
		float i= 0.0f;
		float rate= 1.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector2.Lerp(startPos, endPos,i);
			yield return null; 
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
