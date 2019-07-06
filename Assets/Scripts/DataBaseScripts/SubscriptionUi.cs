using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubscriptionUi : MonoBehaviour
{
    [SerializeField] private GameObject subcriptionPanel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject blockGame;
    [SerializeField] private Text redeemResultText;
    [SerializeField] private Button redeemButton;
    [SerializeField] private Button backButton;
    [SerializeField] private InputField redeemCodeInput;
    private const string redeemUrl = "http://softvtech.website/ListenIn/php/redeem_code.php";

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
            subcriptionPanel.SetActive(true);
        }
        else if(result == "error")
        {
            errorPanel.SetActive(true);
        }

        blockGame.SetActive(false);
    }

    public void RedeemCode(InputField input)
    {
        StartCoroutine(RedeemCodeCoroutine(input.text));
    }

    private IEnumerator RedeemCodeCoroutine(string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.UserId);
        form.AddField("code", code);

        using (WWW www = new WWW(redeemUrl, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
            }
            else
            {
                if (www.text == "bien")
                {
                    Debug.Log("CODE REMDEEMed");
                    subcriptionPanel.SetActive(false);
                }
                else if (www.text == "in_use")
                {
                    Debug.Log("CODE IN USE");
                    redeemCodeInput.interactable = true;
                    redeemButton.interactable = true;
                    backButton.interactable = true;
                    redeemResultText.text = "Code already in use. Try another.";
                    redeemResultText.gameObject.SetActive(true);
                    yield return new WaitForSeconds(4);
                    redeemResultText.gameObject.SetActive(false);
                }
                else if (www.text == "code_not_exist")
                {
                    Debug.Log("CODE doesnt exist");
                    redeemCodeInput.interactable = true;
                    redeemButton.interactable = true;
                    backButton.interactable = true;
                    redeemResultText.text = "Code invalid. Try another.";
                    redeemResultText.gameObject.SetActive(true);
                    yield return new WaitForSeconds(4);
                    redeemResultText.gameObject.SetActive(false);
                }
            }
        }
    }
}
