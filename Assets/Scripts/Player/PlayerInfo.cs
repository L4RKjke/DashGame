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
    }

    private void OnDisable()
    {
        _score = 0;
    }

    public void AddPoint()
    {
        Debug.Log("scoreUpdate " + _score);

        _score++;

        if (isLocalPlayer)
        {
            CmdUpdateScore(_score);
            ScoreUpdated?.Invoke(_score);
        }
    }

    private void SetName(string name)
    {
        _name = name;
    }

    [Command]
    private void CmdUpdateScore(int newScore)
    {
        _score = newScore;
        _textMeshProUGUI.text = _score.ToString();
        ScoreUpdated?.Invoke(_score);
        RpcUpdateScore(_score);
    }

    [ClientRpc]
    private void RpcUpdateScore(int newScore)
    {
        _score = newScore;
        _textMeshProUGUI.text = _score.ToString();
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
}