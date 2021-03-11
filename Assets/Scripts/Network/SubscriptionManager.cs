using UnityEngine;
using System.Collections;

public class SubscriptionManager : MonoBehaviour
{
    public static SubscriptionManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void RequestSubscription()
    {
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.IdUser);

        NetworkManager.SendDataServer(form, NetworkUrl.ServerUrlSubscriptionCheck, "test", FindObjectOfType<SubscriptionUi>().RequestSubscriptionCallback);
    }
}
