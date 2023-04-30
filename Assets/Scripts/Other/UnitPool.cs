using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UnitPool : NetworkBehaviour
{
    private List<Player> _units = new List<Player>();

    public void Add(Player player)
    {
        _units.Add(player);
    }
}
