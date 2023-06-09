using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private List<NetworkStartPosition> _spawnPoints;

    private int _spawned = 0;

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);

        ShufleList(_spawnPoints);
        _spawned = 0;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        InitPlayer(conn);
    }

    private Transform GetRandomSpawnPoint(int id)
    {
        return _spawnPoints[id].transform;
    }

    private void ShufleList(List<NetworkStartPosition> list)
    {
        System.Random random = new System.Random();
        list.Sort((x, y) => random.Next(-1, 2));
    }

    private void InitPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab, GetRandomSpawnPoint(_spawned).position, Quaternion.identity);

        _spawned++;

        if (player.TryGetComponent(out PlayerInfo Info))
        {
            Info.SetName("player" + _spawned + ":");
        }

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
