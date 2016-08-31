using UnityEngine;
using System.Collections;

public class DuckTarget : MonoBehaviour {

	public float rotationAmount;
	public float rotationSpeed;

	//an absolute value
	public float maxDistance;
	private int direction = 1;

	public float translationSpeed;
    public float minTranslationSpeed = 0.8f;
    public float maxTranslationSpeed = 1.2f;

    public float randomScaleSecondsLow = 1.0f;
    public float randomScaleSecondsHigh = 3.0f;
    public float minLocalScale = 0.8f;
    public float maxLocalScale = 1.2f;
    private Vector3 initialScale;

    private bool invertedDirection;

	// Use this for initialization
	void Start () {
        initialScale = transform.localScale;
        float randomTime = Random.Range(randomScaleSecondsLow, randomScaleSecondsHigh);
        float randomScale= Random.Range(minLocalScale, maxLocalScale);
        StartCoroutine(ScaleTarget(randomTime, randomScale));
	}

    IEnumerator ScaleTarget(float randomTime, float randomScale)
    {
        float timer = 0.0f;
        float fraction = 0.0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = initialScale * randomScale;

        while (fraction < 1.0f)
        {
            fraction = timer / randomTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, fraction);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        randomTime = Random.Range(randomScaleSecondsLow, randomScaleSecondsHigh);
        randomScale = Random.Range(minLocalScale, maxLocalScale);

        StartCoroutine(ScaleTarget(randomTime, randomScale));

    }

	// Update is called once per frame
	void Update () {

		transform.Rotate(Vector3.forward, rotationAmount *  rotationSpeed * Time.deltaTime);
		float currXpos = transform.position.x;

		if( Mathf.Abs(currXpos) > maxDistance && !invertedDirection)
		{
            invertedDirection = false;
            Invoke("DirectionChangeAcknoledge", 2.0f);
            direction *= -1;
            translationSpeed = Random.Range(minTranslationSpeed, maxTranslationSpeed);
        }

		transform.position += Vector3.right * direction * translationSpeed * Time.deltaTime;

	}

    private void DirectionChangeAcknoledge()
    {
        invertedDirection = false;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
