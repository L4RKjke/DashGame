using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
/// Extention for player
[RequireComponent(typeof(NetworkMatch))]
public class Client : NetworkBehaviour
{
    public static Client LocalPlayer;

    [SyncVar] private string _matchId;

    private NetworkMatch _networkMatch;

    private readonly string _gameSceneName = "OnlineGameScene";

    private void Start()
    {
        _networkMatch = GetComponent<NetworkMatch>();

        if (isLocalPlayer)
        {
            LocalPlayer = this;
        }
        else
        {
            MainMenu.Instance.SpawnUIPrefab();
        }
    }

    public void HostGame()
    {
        string matchId = MatchMaker.GetRandomMatchId();
        Debug.Log(matchId);
        CmdHostGame(matchId);
    }

    [Command]
    private void CmdHostGame(string matchId)
    {
        _matchId = matchId;

        if (MatchMaker.Instance.TryHostGame(matchId, gameObject))
        {
            _networkMatch.matchId = matchId.ToGuid();
            TargetHostGame(true, matchId);
        }
        else
        {
            TargetHostGame(false, matchId);
        }
    }

    [TargetRpc]
    private void TargetHostGame(bool success, string matchId)
    {
        MainMenu.Instance.HostSuccess(success);
    }

    public void JoinGame(string inputID)
    {
        CmdJoinGame(inputID);
    }

    [Command]
    private void CmdJoinGame(string matchId)
    {
        _matchId = matchId;

        if (MatchMaker.Instance.TryHostGame(matchId, gameObject))
        {
            _networkMatch.matchId = matchId.ToGuid();
            TargetJoinGame(true, matchId);
        }
        else
        {
            TargetJoinGame(false, matchId);
        }
    }


    [TargetRpc]
    private void TargetJoinGame(bool success, string matchId)
    {
        MainMenu.Instance.JoinSuccess(success);
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    public void CmdBeginGame()
    {
        MatchMaker.Instance.BeginGame(_matchId);
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame()
    {
        SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Additive);
    }
}
