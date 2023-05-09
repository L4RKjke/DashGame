using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;

public class GameRestarter : NetworkBehaviour
{
    [SerializeField] private UnitPool _unitPool;
    [SerializeField] private UiMessager _finalMessager;

    private readonly string _loadMethodName = nameof(LoadCurrentScene);
    private readonly int _loadTimeout = 5;
    private readonly int _winScore = 3;

    private string _name;

    public string Name => _name;

    private void OnEnable()
    {
        _unitPool.PlayerAdded += OnPlayerAdded;
    }

    private void OnDisable()
    {
        _unitPool.PlayerAdded -= OnPlayerAdded;
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

    private void OnPlayerAdded(PlayerInfo info)
    {
        info.ScoreUpdated += WaitForWinner;
        _name = info.Name;
    }

    private void WaitForWinner(int score)
    {
        if (score == _winScore)
        {
            RestartGame();
            _finalMessager.ShowMessage(_name);
        }
    }
}