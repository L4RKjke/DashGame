using TMPro;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(RectTransform))]

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerName;

    private PlayerInfo _playerInfo;
    private RectTransform _rectTransform;

    public RectTransform RectTransform => _rectTransform;

    public void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        _playerInfo.ScoreUpdated -= SetScore;
    }

    public void Init(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;
        SetName(_playerInfo.Name);
        _playerInfo.ScoreUpdated += SetScore;
    }

    public void SetScore(int score)
    {
        _playerScore.text = score.ToString();

        if (isLocalPlayer)
        {
            CmdSetScore(score);
        }
    }

    public void SetName(string name)
    {
        _playerName.text = name;
    }

    [Command]
    private void CmdSetScore(int score)
    {
        _playerScore.text = score.ToString();

        RpcSetScore(score);
    }

    [ClientRpc]
    private void RpcSetScore(int score)
    {
        _playerScore.text = score.ToString();
        Debug.Log(_playerScore);
    }
}
