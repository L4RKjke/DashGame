using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private Button _join;
    [SerializeField] private TMP_InputField _JoinInput;
    [SerializeField] private CustomNetworkManager _networkManager;
    [SerializeField] private Canvas _lobbyCanvas;

    public static MainMenu Instance;

    private void Start()
    {
        Instance = this;
    }

    private void OnEnable()
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
        /*_networkManager.StartHost();*/
        _JoinInput.interactable = false;
        _host.interactable = false;
        _join.interactable = false;
        Client.LocalPlayer.HostGame();
    }

    public void HostSuccess(bool success)
    {
        if (success)
        {
            _lobbyCanvas.enabled = true;
        }
        else
        {
            _JoinInput.interactable = true;
            _host.interactable = true;
            _join.interactable = true;
        }
    }

    private void OnJoinButtonClick()
    {
        _JoinInput.interactable = false;
        _host.interactable = false;
        _join.interactable = false;
        Client.LocalPlayer.JoinGame();
        /*_networkManager.StartClient();*/
    }

    public void JoinSuccess(bool success)
    {
        if (success)
        {
            _lobbyCanvas.enabled = true;
        }
        else
        {
            _JoinInput.interactable = true;
            _host.interactable = true;
            _join.interactable = true;
        }
    }
}
