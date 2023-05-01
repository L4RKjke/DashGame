using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BattleUI : NetworkBehaviour
{
    [SerializeField] private FieldPlayerTrigger _trigger;
    [SerializeField] private PlayerUI _playerUITemplete;
    [SerializeField] private Transform _content;
    [SerializeField] private GameRestarter _gameRestarter;
    [SerializeField] private FinalMessager _gameEnder;

    private List<PlayerUI> _playerUiList = new List<PlayerUI>();

    private void OnEnable()
    {
        _trigger.PlayerAdded += CreatePlayerUI;
    }

    private void OnDisable()
    {
        _trigger.PlayerAdded -= CreatePlayerUI;
    }

    private void Update()
    {
        OnScoreChanged();
    }

    private void OnScoreChanged()
    {
        SetScore();

        if (!isLocalPlayer) return;

        CmdSetNewScore();
    }

    [Command]
    private void CmdSetNewScore()
    {
        SetScore();

        if (isServer)
            RpcSetNewScore();
    }

    [ClientRpc]
    private void RpcSetNewScore()
    {
        SetScore();
    }

    private void SetScore()
    {
        for (int i = 0; i < _trigger.Lenght(); i++)
        {
            var getPlayer = _trigger.GetPlayer(i);

            _playerUiList[i].SetScore(getPlayer.Score);
            _playerUiList[i].SetName(getPlayer.Name); ;

            if (getPlayer.Score == 3)
            {
                _gameEnder.ShowMessage(getPlayer.Name);
                _gameRestarter.RestartGame();
            }
        }
    }

    private void CreatePlayerUI()
    {
        var newPlayerUI = Instantiate(_playerUITemplete, _content);
        newPlayerUI.RectTransform.localScale = new Vector3(1, 1, 1);
        _playerUiList.Add(newPlayerUI);
    }
}