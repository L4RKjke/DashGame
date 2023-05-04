using UnityEngine;
using Mirror;

public class BattleUI : NetworkBehaviour
{
    [SerializeField] private UnitPool _unitPool;
    [SerializeField] private PlayerUI _playerUITemplete;
    [SerializeField] private Transform _content;

    private void OnEnable()
    {
        _unitPool.PlayerAdded += CreatePlayerUI;
    }

    private void OnDisable()
    {
        _unitPool.PlayerAdded -= CreatePlayerUI;
    }

    private void CreatePlayerUI(PlayerInfo playerInfo)
    {
        var newPlayerUI = Instantiate(_playerUITemplete, _content);
        newPlayerUI.RectTransform.localScale = new Vector3(1, 1, 1);
        newPlayerUI.Init(playerInfo);
    }
}