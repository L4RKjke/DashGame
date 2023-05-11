using UnityEngine;
using System;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private Player _player;

    [SyncVar(hook = nameof(OnNameUpdated))]
    private string _name;
    private int _score;

    public Action<PlayerInfo> ScoreUpdated;

    public string Name => _name;

    public int Score => _score;

    private void OnDisable()
    {
        _score = 0;
    }

    [Command]
    public void CmdAddPoint()
    {
        _score++;
        RpcUpdateScore(_score);
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void OnNameUpdated(string oldName,string newName)
    {
        _name = newName;
    }

    [ClientRpc]
    private void RpcUpdateScore(int newScore)
    {
        _score = newScore;
        ScoreUpdated?.Invoke(this);
    }
}