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

    public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();

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

    public void SetScore(PlayerInfo info)
    {
        _playerScore.text = info.Score.ToString();

        if (isLocalPlayer)
        {
            CmdSetScore(info.Score);
        }
    }

    public void SetName(string newValue)
    {
        _playerName.text = newValue;
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
