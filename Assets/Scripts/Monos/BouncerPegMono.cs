using UnityEngine;
using System.Collections;

public class BouncerPegMono : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collision)
	{
	    float bounceX = Random.Range(-400f, 400f);
        float bounceY = Random.Range ( -400f , 400f );
//        Debug.Log ( "Bouncer" );
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce ( new Vector2 ( bounceX , bounceY ) );

    }

    
}
