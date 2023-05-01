using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private Button _join;
    [SerializeField] private NetworkManager _networkManager;

    private void Start()
    {
        _host.onClick.AddListener(OnHostButtonClick);
        _join.onClick.AddListener(OnJoinButtonClick);
    }

    private void OnDisable()
    {
        _host.onClick.RemoveListener(OnHostButtonClick);
        _join.onClick.RemoveListener(OnJoinButtonClick);
    }

    private void OnHostButtonClick()
    {
        _networkManager.StartHost();
    }

    private void OnJoinButtonClick()
    {
        _networkManager.StartClient();
    }
}
