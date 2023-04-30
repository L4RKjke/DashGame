using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameRestarter : NetworkBehaviour
{
    private readonly string _loadMethodName = "LoadCurrentScene";
    private readonly int _loadTimeout = 5;

    public void RestartGame()
    {
        if (!isServer) return;

        Invoke(_loadMethodName, _loadTimeout);
    }

    private void LoadCurrentScene()
    {
        NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }
}
