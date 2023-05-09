using UnityEngine;
using TMPro;
using Mirror;

public class FinalMessager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private TextMeshProUGUI _server;

    private void Start()
    {
        if (NetworkServer.active && NetworkClient.active)
        {
            _server.text = "Хост";
        }
    }

    public void ShowMessage(string name)
    {
        _textMeshProUGUI.text = name + " WIN!";
    }
}
