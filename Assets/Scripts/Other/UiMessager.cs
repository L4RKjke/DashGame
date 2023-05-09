using UnityEngine;
using TMPro;
using Mirror;

public class UiMessager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private TextMeshProUGUI _playerName;

    private void Start()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            _playerName.text = "Host";
        }
        {
            _playerName.text = "Client";
        }
    }

    public void ShowMessage(string name)
    {
        _textMeshProUGUI.text = name + " WIN!";
    }
}
