using UnityEngine;
using System;
using Mirror;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private Player _player;

    [SyncVar(hook = nameof(OnNameUpdated))]
    private string _name;
    private int _score;

    public Action<int> ScoreUpdated;

    public string Name => _name;

    public int Score => _score;

    private void OnDisable()
    {
        _score = 0;
    }

    public void AddPoint()
    {
        _score++;

        if (isLocalPlayer)
        {
            CmdUpdateScore(_score);
            ScoreUpdated?.Invoke(_score);
        }
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void OnNameUpdated(string oldName,string newName)
    {
        _name = newName;
    }

    [Command]
    private void CmdUpdateScore(int newScore)
    {
        _score = newScore;
        ScoreUpdated?.Invoke(_score);
        RpcUpdateScore(_score);
    }

    [ClientRpc]
    private void RpcUpdateScore(int newScore)
    {
        _score = newScore;
        ScoreUpdated?.Invoke(_score);
    }

    private string GenerateName()
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] name = new char[10];

        for (int i = 0; i < 10; i++)
        {
            name[i] = chars[random.Next(chars.Length)];
        }

        return new String(name);
    }

    [Command]
    private void CmdUpdateName(string name)
    {
        Debug.Log("cmdUpdateName");
        _name = name;
        RpcUpdateName(name);
    }

    [ClientRpc]
    private void RpcUpdateName(string name)
    {
        Debug.Log("RpcUpdateName");
        _name = name;
    }
}