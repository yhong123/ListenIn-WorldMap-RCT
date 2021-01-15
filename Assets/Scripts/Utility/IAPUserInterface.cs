using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPUserInterface : MonoBehaviour
{
	[SerializeField] private bool debugSubscribed;
	[SerializeField] private CanvasGroup panelSubscription;
	[SerializeField] private CanvasGroup panelSubscriptionBad;
	[SerializeField] private CanvasGroup panelSubscriptionGood;

	private void Awake()
	{
		IAPManager.OnPurchase += IAPManager_OnPurchase;
	}

	private void OnDestroy()
	{
		IAPManager.OnPurchase -= IAPManager_OnPurchase;
	}

	private void IAPManager_OnPurchase(bool success)
	{
		if (success)
		{
			Utility.Instance.SetElementVisibility(panelSubscriptionGood, true);
			Utility.Instance.SetElementVisibility(panelSubscription, false);
		}
		else
		{
			Utility.Instance.SetElementVisibility(panelSubscriptionBad, true);
		}
	}

	private void Start()
	{
		IAPManager.Instance.CheckSubscription(CheckSubscriptionCallback);
	}

	private void CheckSubscriptionCallback(string response)
	{
		if (response == "true" || IAPManager.Instance.GetIsSubscribed() || debugSubscribed)
		{
			return;
		}
		Utility.Instance.SetElementVisibility(panelSubscription, true);
	}

	public void PurchaseSubscription()
	{
		IAPManager.Instance.BuySubscription();
	}

	public void OnPanelSubscriptionClicked()
	{
		Utility.Instance.SetElementVisibility(panelSubscriptionGood, false);
		Utility.Instance.SetElementVisibility(panelSubscriptionBad, false);
	}

	public void OnButtonLogOut()
	{
		AppManager.Instance.LogOut();
	}
}