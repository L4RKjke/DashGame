using UnityEngine;
using Mirror;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;

    private readonly string _speed = "Speed";
    private readonly string _dash = "Dash";

    private void OnEnable()
    {
        _player.SpeedChanged += OnMove;
        _player.DashStateChanged += OnDashStateChanged;
    }

    private void OnDisable()
    {
        _player.SpeedChanged -= OnMove;
        _player.DashStateChanged -= OnDashStateChanged;
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
