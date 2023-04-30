using TMPro;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(RectTransform))]

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerName;

    public RectTransform RectTransform => GetComponent<RectTransform>();

    public void SetScore(int score)
    {
        _playerScore.text = score.ToString();
    }

    public void SetName(string name)
    {
        _playerName.text = name;
    }
}
