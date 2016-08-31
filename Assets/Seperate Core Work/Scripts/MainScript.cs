using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	/*public AudioClip clip1;
	private AudioClip clip2;
	private GUIStyle styleScore;
	string strScore = "";*/

	// Use this for initialization
	void Start () 
	{
		//styleScore = new GUIStyle ();
		//styleScore.fontSize = 40;
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*for (var i = 0; i < Input.touchCount; ++i) 
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began) 
			{
				RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if (hitInfo)
				{
					strScore = hitInfo.transform.gameObject.name;
					Debug.Log( hitInfo.transform.gameObject.name );
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.

					GameObject.Find("Item1").GetComponent<SpriteRenderer> ().sprite = Resources.Load("Images/gift", typeof(Sprite)) as Sprite;
				}
				else
				{
					strScore = "hitInfo = null";
					Debug.Log( "hitInfo = null" );
				}
			}
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			Debug.Log ("Clicked");
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if(hitInfo)
			{
				Debug.Log( hitInfo.transform.gameObject.name );
				// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				GameObject.Find("Item1").GetComponent<SpriteRenderer> ().sprite = Resources.Load("Images/gift", typeof(Sprite)) as Sprite;

			}
		}
		/*if(Input.GetButtonDown("Jump"))
		{
			if(audio.isPlaying == true)
			{
				audio.Stop();
			}
			else
			{
				//aCubeOnSlot = Resources.Load("sounds/cube_on_slot",typeof(AudioClip));
				clip2 = Resources.Load("car") as AudioClip;
				audio.clip = clip2;
				audio.Play();
				Debug.Log( "play audio" );
			}
		}*/	
	}

}
