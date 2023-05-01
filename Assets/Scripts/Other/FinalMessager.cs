using UnityEngine;
using TMPro;
using Mirror;

public class FinalMessager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void ShowMessage(string name)
    {
        _textMeshProUGUI.text = name + " WIN!";
    }
}
