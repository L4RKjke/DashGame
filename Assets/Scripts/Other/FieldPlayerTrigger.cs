using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class FieldPlayerTrigger : NetworkBehaviour
{
    [SerializeField] private BattleUI _ui;

    private List<PlayerInfo> _units = new List<PlayerInfo>();
    public Action PlayerAdded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInfo player))
        {
            Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInfo player))
        {
            Remove(player);
            _units.Clear();
        }    
    }

    public void Add(PlayerInfo player)
    {
        PlayerAdded?.Invoke();
        _units.Add(player);
    }

    public void Remove(PlayerInfo player)
    {
        _units.Remove(player);
    }

    public int Lenght()
    {
        return _units.Count;
    }

    public PlayerInfo GetPlayer(int id)
    {
        return _units[id];
    }
}