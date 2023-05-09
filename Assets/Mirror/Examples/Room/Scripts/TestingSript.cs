using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TestingSript : MonoBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private NetworkManager _networkManager;

    private void OnEnable()
    {
        _host.onClick.AddListener(() => _networkManager.StartHost());
    }

    private void OnDisable()
    {
        
    }
}
