using UnityEngine;
using Mirror;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private IdleState _idleState;
    [SerializeField] private AttackState _attackState;
    [SerializeField] private MoveState _moveState;

    private readonly string _speed = "Speed";
    private readonly string _dash = "Dash";

    private void OnEnable()
    {
        _idleState.IdleStateActivated += OnIdle;
        _attackState.AttackStateChanged += OnDashStateChanged;
        _moveState.SpeedChanged += OnMove;
    }

    private void OnDisable()
    {
        _idleState.IdleStateActivated -= OnIdle;
        _attackState.AttackStateChanged -= OnDashStateChanged;
        _moveState.SpeedChanged += OnMove;
    }

    private void OnMove()
    {
        _animator.SetFloat(_speed, _player.PlayerMover.Speed);
    }

    private void OnIdle()
    {
        _animator.SetFloat(_speed, 0);
    }

    private void OnDashStateChanged(bool state)
    {
        _animator.SetBool(_dash, state);
    }
}
