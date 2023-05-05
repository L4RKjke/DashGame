using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MainMenu : NetworkBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private Button _join;
    [SerializeField] private Button _startGame;
    [SerializeField] private TMP_InputField _JoinInput;
    [SerializeField] private Canvas _lobbyCanvas;
    [SerializeField] private GameObject _playerContent;
    [SerializeField] private GameObject _UiPrefab;

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
            SpawnUIPrefab();
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
        Client.LocalPlayer.JoinGame(_JoinInput.text);
        Debug.Log(_JoinInput.text);
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

    public void OnStartButtonClick()
    {
        Client.LocalPlayer.StartGame();
    }

    public void SpawnUIPrefab()
    {
        GameObject prefab = Instantiate(_UiPrefab, _playerContent.transform);
    }
}
