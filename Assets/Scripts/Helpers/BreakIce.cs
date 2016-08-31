using UnityEngine;
using System.Collections;

public class BreakIce : MonoBehaviour {

	public Sprite BreakedIce;
	private SpriteRenderer spriteRenderer;
	enum IceState{Full,Breaked,Deleted};

	private IceState icestate;

	private bool blinkIce;
	public int blinkCount = 5;
	public float blinkTime = 1f;
	private bool animationEnded = false;
	private float currTime = 0f;
	private Color currColor;
	private Color targetColor;
	private bool direction = false;

    Collider2D collider;
    public bool reactivate = false;
    public float reactivateTime = 7.5f;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        icestate = IceState.Full;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Coin")
		{
			switch (icestate) {
			case IceState.Full:
				if(BreakedIce != null)
				{
					spriteRenderer.sprite = BreakedIce;
					icestate = IceState.Breaked;
				}
				else
				{
					blinkIce = true;
					icestate = IceState.Deleted;
				}
				break;
			case IceState.Breaked:
				blinkIce = true;
				icestate = IceState.Deleted;
				break;
			default:
				break;
			}
		}
	}

	IEnumerator Blink()
	{
		while(currTime <= blinkTime)
		{
			currTime += Time.deltaTime;
			spriteRenderer.color = Color.Lerp(currColor,targetColor,currTime / blinkTime);
			yield return null;
		}

		currColor = spriteRenderer.color = targetColor;
		
		if (blinkCount > 0)
		{
			blinkCount--;
			direction = !direction;
			targetColor.a = direction ? 1f : 0.0f;
			currTime = 0.0f;
			StartCoroutine(Blink());
		}
		else
		{
			animationEnded = true;
		}
	}

    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(reactivateTime);
        collider.enabled = true;
        spriteRenderer.enabled = true;
        Color col = spriteRenderer.color;
        col.a = 1.0f;
        //this.gameObject.SetActive(true);
        icestate = IceState.Full;
    }


    // Update is called once per frame
    void Update () {
		if(blinkIce)
		{
			blinkIce = false;
			currColor = spriteRenderer.color;
			targetColor = currColor;
			targetColor.a = 0.1f;
			StartCoroutine(Blink ());
		}
		else if(animationEnded)
		{
			animationEnded = false;
            if(reactivate)
            {
                StartCoroutine(Reactivate());
            }
            spriteRenderer.enabled = false;            
            collider.enabled = false; 
			//this.gameObject.SetActive(false);
		}
	}
}
