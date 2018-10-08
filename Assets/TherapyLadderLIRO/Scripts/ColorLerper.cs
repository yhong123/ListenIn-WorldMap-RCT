using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorLerper : MonoBehaviour {

    private Image currImage;

    public void StopLerp()
    {
        StopAllCoroutines();
        currImage.color = new Color(165.0f / 255.0f, 165.0f / 255.0f, 165.0f / 255.0f);
    }
    public void StartLerp()
    {
        StartCoroutine(LerpBetweenColor());
    }

    void Start()
    {
        currImage = GetComponent<Image>();
    }

    IEnumerator LerpBetweenColor()
    {
        while (true)
        {
            currImage.color = Color.Lerp(new Color(165.0f / 255.0f, 165.0f / 255.0f, 165.0f / 255.0f), new Color(220.0f / 255.0f, 220.0f / 255.0f, 220.0f / 255.0f), Mathf.PingPong(Time.time,1));
            yield return new WaitForEndOfFrame();
        }
    }
}
