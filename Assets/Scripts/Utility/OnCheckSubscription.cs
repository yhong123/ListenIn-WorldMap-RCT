using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnCheckSubscription : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private UnityEvent EventSubscriptionGood;
    private IAPUserInterface iAPUserInterface;

    private void Awake()
    {
        iAPUserInterface = FindObjectOfType<IAPUserInterface>();
    }

	public void OnPointerClick(PointerEventData eventData)
	{
        iAPUserInterface.SetPanelLoading(true);
        IAPManager.Instance.CheckSubscription(CheckSubscriptionCallback);
    }

    private void CheckSubscriptionCallback(string response)
    {
        if (response == "true" || IAPManager.Instance.GetIsSubscribed())
        {
            EventSubscriptionGood.Invoke();
            return;
        }
        iAPUserInterface.SetPanelLoading(false);
        iAPUserInterface.SetPanelSubscription(true);
    }
}
