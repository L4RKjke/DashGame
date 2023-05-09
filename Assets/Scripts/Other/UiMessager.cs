using UnityEngine;
using TMPro;
using Mirror;

public class UiMessager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private TextMeshProUGUI _playerName;

    private void Update()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            _playerName.text = "Хост";
        }
    }

    public void ShowMessage(string name)
    {
        _textMeshProUGUI.text = name + " WIN!";
    }
}
