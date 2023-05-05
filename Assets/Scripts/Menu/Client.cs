using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkMatch))]
public class Client : NetworkBehaviour
{
    public static Client LocalPlayer;

    [SyncVar] public string MatchId;

    private NetworkMatch _networkMatch;

    private void Start()
    {
        if (isLocalPlayer)
        {
            LocalPlayer = this;
        }

        _networkMatch = GetComponent<NetworkMatch>();
    }

    public void HostGame()
    {
        string matchId = MatchMaker.GetRandomMatchId();
        CmdHostGame(matchId);
    }

    [Command]
    private void CmdHostGame(string matchId)
    {
        MatchId = matchId;

        if (MatchMaker.Instance.TryHostGame(matchId, gameObject))
        {
            Debug.Log("gameHosted cuccess");
            _networkMatch.matchId = matchId.ToGuid();
            TargetHostGame(true, matchId);
        }
        else
        {
            Debug.Log("game not Hosted");
            TargetHostGame(false, matchId);
        }
    }

    [TargetRpc]
    private void TargetHostGame(bool success, string matchId)
    {
        MainMenu.Instance.HostSuccess(success);
    }

    public void JoinGame()
    {
        string matchId = MatchMaker.GetRandomMatchId();
        CmdJoinGame(matchId);
    }

    [Command]
    private void CmdJoinGame(string matchId)
    {
        MatchId = matchId;

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
}
