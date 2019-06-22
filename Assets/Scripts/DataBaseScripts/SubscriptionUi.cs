using UnityEngine;
using System.Collections;

public class SubscriptionUi : MonoBehaviour
{
    [SerializeField] private GameObject subcriptionPanel;

    private void Start()
    {
        SubscriptionManager.Instance.RequestSubscription();
    }

    public void RequestSubscriptionCallback(string result)
    {
        if(result == "true")
        {
            //continue gameplay
            Debug.Log("<b>SUBSCRIPTION:<color=green>VALID</color></b>");
        }
        else if(result == "false")
        {
            //no subscription left
            //subcriptionPanel.SetActive(true);
            Debug.Log("<b>SUBSCRIPTION:<color=red>INVALID</color></b>");
        }
    }
}
