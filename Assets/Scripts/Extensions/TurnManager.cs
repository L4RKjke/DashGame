using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkMatch))]
public class TurnManager : NetworkBehaviour
{
    private List<Client> _players = new List<Client>();

    public NetworkMatch NetworkMatch => GetComponent<NetworkMatch>();

    public void AddPlayer(Client player)
    {
        _players.Add(player);
    }
}