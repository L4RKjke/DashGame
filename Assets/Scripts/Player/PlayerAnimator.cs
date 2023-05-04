using UnityEngine;
using Mirror;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private DashAbility _dashAbility;
    [SerializeField] private AttackState _attackState;

    private readonly string _speed = "Speed";
    private readonly string _dash = "Dash";

    private void OnEnable()
    {
        _player.PlayerMover.SpeedChanged += OnMove;
        _attackState.AttackEnded += OnDashStateChanged;
        _dashAbility.DashStateChanged += OnDashStateChanged;
    }

    private void OnDisable()
    {
        _player.PlayerMover.SpeedChanged -= OnMove;
        _attackState.AttackEnded -= OnDashStateChanged;
        _dashAbility.DashStateChanged -= OnDashStateChanged;
    }

    private void OnMove(float speed)
    {
        _animator.SetFloat(_speed, speed);
    }

    private void OnDashStateChanged(bool state)
    {
        _animator.SetBool(_dash, state);
    }
}
