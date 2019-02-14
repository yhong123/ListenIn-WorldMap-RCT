using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LogOffButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerPrefManager.LogOut();
    }
}
