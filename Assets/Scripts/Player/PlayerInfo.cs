using UnityEngine;
using System;
using Mirror;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private string _name;
    private int _score;

    public Action<int> ScoreUpdated;

    public string Name => _name;

    public int Score => _score;

    private void OnEnable()
    {
        _name = GenerateName();

        if (isServer)
            RpcSetName(_name);
        else
            CmdSetName(_name);

        _player.Dashed += AddPoint;
    }

    private void OnDisable()
    {
        _player.Dashed -= AddPoint;
        _score = 0;
    }

    private void AddPoint()
    {
        _score++;

        if (isServer)
            RpcUpdateScore(_score);
        else
            CmdUpdateScore(_score);

        ScoreUpdated?.Invoke(_score);
    }

    [Command]
    private void CmdSetName(string name)
    {
        _name = name;
    }

    [ClientRpc]
    private void RpcSetName(string name)
    {
        _name = name;
    }

    [Command]
    private void CmdUpdateScore(int newScore)
    {
        _score = newScore;
    }

    [ClientRpc]
    private void RpcUpdateScore(int newScore)
    {
        _score = newScore;
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
}