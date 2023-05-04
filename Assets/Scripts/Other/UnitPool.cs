using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class UnitPool : MonoBehaviour
{
    [SerializeField] private BattleUI _ui;

    private List<PlayerInfo> _units = new List<PlayerInfo>();
    public Action<PlayerInfo> PlayerAdded;

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
    }

    public void Add(PlayerInfo player)
    {
        PlayerAdded?.Invoke(player);
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