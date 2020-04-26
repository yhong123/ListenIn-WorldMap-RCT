using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubscriptionUi : MonoBehaviour
{
    [SerializeField] private GameObject subscriptionPanel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject blockGame;
    [SerializeField] private GameObject subcriptionFinish;
    [SerializeField] private GameObject errorSubscribing;
    [SerializeField] private Text redeemResultText;
    [SerializeField] private Button redeemButton;
    [SerializeField] private Button backButton;
    [SerializeField] private InputField redeemCodeInput;
    private Coroutine displayMessage = null;

    private void Awake()
    {
        blockGame.SetActive(true);
    }

    private void Start()
    {
        SubscriptionManager.Instance.RequestSubscription();
    }

    public void RequestSubscriptionCallback(string result)
    {
        if (result == "true")
        {
            //continue gameplay
            Debug.Log("<b>SUBSCRIPTION:<color=green>VALID</color></b>");
        }
        else if (result == "false")
        {
            //no subscription left
            //subcriptionPanel.SetActive(true);
            Debug.Log("<b>SUBSCRIPTION:<color=red>INVALID</color></b>");
            subscriptionPanel.SetActive(true);
        }
        else if (result == "error")
        {
            errorPanel.SetActive(true);
        }

        blockGame.SetActive(false);
    }

    public void RedeemCode(InputField input)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.UserId);
        form.AddField("code", input.text);
        
        NetworkManager.SendDataServer(form, NetworkUrl.ServerUrlRedeemCode, "test", RedeemCodeCallback);
    }

    public void RedeemCodeCallback(string result)
    {
        if (result == "bien")
        {
            Debug.Log("CODE REMDEEMed");
            subscriptionPanel.SetActive(false);
        }
        else if (result == "in_use")
        {
            Debug.Log("CODE IN USE");
            redeemCodeInput.interactable = true;
            redeemButton.interactable = true;
            backButton.interactable = true;

            if(displayMessage != null)
            {
                StopCoroutine(displayMessage);
            }
            displayMessage = StartCoroutine(DisplayMessageAndWait("Code already in use. Try another."));
        }
        else if (result == "code_not_exist")
        {
            Debug.Log("CODE doesnt exist");
            redeemCodeInput.interactable = true;
            redeemButton.interactable = true;
            backButton.interactable = true;

            if (displayMessage != null)
            {
                StopCoroutine(displayMessage);
            }
            displayMessage = StartCoroutine(DisplayMessageAndWait("Code invalid. Try another."));
        }
    }

    private IEnumerator DisplayMessageAndWait(string text)
    {
        redeemResultText.text = text;
        redeemResultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        redeemResultText.gameObject.SetActive(false);
        displayMessage = null;
    }

    public void ExtendSubscription(int period)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.UserId);
        form.AddField("period", period);

        NetworkManager.SendDataServer(form, NetworkUrl.ServerUrlExtendSubscription, "test", ExtendSubscription);
    }

    private void ExtendSubscription(string result)
    {
        if(result == "error")
        {
            errorSubscribing.SetActive(true);
        }
        else if(result == "bien")
        {
            subscriptionPanel.SetActive(false);
            StartCoroutine(FinishSubscription());
        }
    }

    private IEnumerator FinishSubscription()
    {
        subcriptionFinish.SetActive(true);
        yield return new WaitForSeconds(4);
        subcriptionFinish.SetActive(false);
    }
}
