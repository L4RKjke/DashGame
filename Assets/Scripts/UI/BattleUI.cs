using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BattleUI : NetworkBehaviour
{
    [SerializeField] private FieldPlayerTrigger _trigger;
    [SerializeField] private PlayerUI _playerUITemplete;
    [SerializeField] private Transform _content;
    [SerializeField] private GameRestarter _gameRestarter;
    [SerializeField] private GameEnder _gameEnder;

    [SyncVar(hook = nameof(OnScoreChanged))]

    private int score;

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
        OnScoreChanged(0, 0);
    }

    private void OnScoreChanged(int oldScore, int newScore)
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
            _playerUiList[i].SetScore(_trigger.GetPlayer(i).Score);
            _playerUiList[i].SetName(_trigger.GetPlayer(i).Name); ;

            if (_trigger.GetPlayer(i).Score == 3)
            {
                _gameEnder.ShowMessage(_trigger.GetPlayer(i).Name);
                _gameRestarter.RestartGame();
            }
        }
    }

    private void CreatePlayerUI()
    {
        var newPlayerUI = Instantiate(_playerUITemplete);
        newPlayerUI.gameObject.transform.SetParent(_content);
        newPlayerUI.RectTransform.localScale = new Vector3(1, 1, 1);
        _playerUiList.Add(newPlayerUI);
    }
}