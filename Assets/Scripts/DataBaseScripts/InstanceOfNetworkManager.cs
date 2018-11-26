using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceOfNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject NetworkManagerInstance;

    private void Awake()
    {
       if(NetworkManager.Instance == null)
        {
            Instantiate(NetworkManagerInstance);
        }
    }
}
