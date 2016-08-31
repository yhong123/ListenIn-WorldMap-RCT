using UnityEngine;
using System.Collections;

public class RotatingLoli : MonoBehaviour {
	public GameObject lolipop;
	private bool isMoving = false;
	public float speedRot;
	private float time = 60.0f;
	public Transform loliTrans;

	// Use this for initialization
	void Start () {
		//lolipop = GetComponent<Rigidbody2D> ();
		//StartCoroutine (placeDiff());
	}
	

	private IEnumerator placeDiff()
	{

			do {
				//loliTrans.transform.Translate (new Vector3 (-2.0f * Time.deltaTime * speed, 0.0f, 0.0f));
				float rotX = transform.rotation.x;
				float rotY = transform.rotation.y;
				
				loliTrans.transform.Rotate(new Vector3(rotX,rotY,10.0f)*speedRot*Time.deltaTime, Space.Self);
				//loliTrans.transform.RotateAround(new Vector3(rotX,rotY,10.0f), new Vector3(rotX,rotY,10.0f), speedRot*Time.deltaTime);
				
				yield return new WaitForSeconds (time);
		} while(!isMoving && Time.deltaTime < time);

	}

	// Update is called once per frame
	void Update () {
		lolipop.transform.Rotate(new Vector3(0.0f,0.0f,speedRot*Time.deltaTime*10));
		StartCoroutine (placeDiff());

	}
}
