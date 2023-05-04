using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;

public class GameRestarter : NetworkBehaviour
{
    [SerializeField] private UnitPool _unitPool;
    [SerializeField] private FinalMessager _finalMessager;

    private readonly string _loadMethodName = nameof(LoadCurrentScene);
    private readonly int _loadTimeout = 5;

    private void Update()
    {
        Debug.Log("Count: " + _unitPool.Lenght());

        for (int i = 0; i < _unitPool.Lenght(); i++)
        {
            _unitPool.GetPlayer(i).ScoreUpdated += WaitForWinner;
        }
    }



    public void RestartGame()
    {
        if (!isServer) return;

        Invoke(_loadMethodName, _loadTimeout);
    }

    private void LoadCurrentScene()
    {
        NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    private void WaitForWinner(int score)
    {

    }
}