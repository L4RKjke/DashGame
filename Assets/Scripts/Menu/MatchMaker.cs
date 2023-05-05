using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class Match
{
    public string MatchId;
    public List<GameObject> _clients = new List<GameObject>();

    public Match (string matchId, GameObject player)
    {
        MatchId = matchId;
        _clients.Add(player);
    }

    public Match() { }
}

public class MatchMaker : NetworkBehaviour
{
    public static MatchMaker Instance;

    public SyncList<Match> Matches = new SyncList<Match>();
    public SyncList<string> MatchIds = new SyncList<string>();

    private void Start()
    {
        Instance = this;
    }

    public bool TryHostGame(string matchId, GameObject player)
    {
        if (!MatchIds.Contains(matchId))
        {
            MatchIds.Add(matchId);
            Matches.Add(new Match(matchId, player));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryJoinGame(string matchId, GameObject player)
    {
        if (MatchIds.Contains(matchId))
        {
            for (int i = 0; i < Matches.Count; i++)
            {
                if (Matches[i].MatchId == matchId)
                {
                    Matches[i]._clients.Add(player);
                    break;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public static string GetRandomMatchId()
    {
        string id = string.Empty;

        for (int i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);

            if (random < 26)
            {
                id += (char)(random + 65);
            }
            else
            {
                id += (random - 26).ToString();
            }
        }

        return id;
    }
}

public static class MatchExtention
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}
