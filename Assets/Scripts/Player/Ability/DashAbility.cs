using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class DashAbility : Ability
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _timeOfInvulnerability;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInfo _playerInfo;

    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashSpeed = 40;
    private Coroutine _dashCoroutine;
    private Coroutine _diactivateRoutine;

    private readonly int _dashReloadTime = 1;

    public Action<HealthStatus> HealthChanged;
    public Action<float> SpeedChanged;
    public Action<bool> DashStateChanged;

    public bool IsDashing => _isDashing;

    private void OnEnable()
    {
        Status = AbilityStatus.Ended;
    }

    private void OnDisable()
    {
        if (_dashCoroutine != null)
            StopCoroutine(_dashCoroutine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            if (player.State == HealthStatus.Damaged) return;

            if (IsDashing)
            {
                player.ApplyDamage();
                _playerInfo.AddPoint();
            }
        }
    }

    public override void ActivateAbiltity()
    {
        if (_isDashing == true) return;

        if (_canDash == false) return;

        if (_diactivateRoutine != null)
            StopCoroutine(_diactivateRoutine);

        _diactivateRoutine = StartCoroutine(DiactivateDashingRoutine());
        _isDashing = true;

        if (isLocalPlayer)
        {
            CmdChangeDashState(true);
        }

        var target = transform.position + (transform.forward * _dashDistance);

        if (isLocalPlayer)
            _dashCoroutine = StartCoroutine(DashRoutine(target));
    }

    private IEnumerator DashRoutine(Vector3 target)
    {
        _rigidbody.velocity = transform.forward * _dashSpeed;
        DashStateChanged.Invoke(_isDashing);
        Status = AbilityStatus.InProgress;

        while (true)
        {
            if (Vector3.Distance(transform.position, target) < 2f)
            {
                _rigidbody.velocity = Vector3.zero;

                _isDashing = false;

                if (isLocalPlayer)
                    CmdChangeDashState(false);

                Status = AbilityStatus.Ended;
                DashStateChanged.Invoke(_isDashing);
                yield break;
            }

            yield return null;
        }
    }

    public override void DeactivateAbiltity()
    {
        if (_dashCoroutine != null)
            StopCoroutine(_dashCoroutine);

        _rigidbody.velocity = Vector3.zero;
        _isDashing = false;

        Status = AbilityStatus.Ended;
    }

    [Command]
    private void CmdChangeDashState(bool state)
    {
        _isDashing = state;
        RpcChagneDashState(state);
    }

    [ClientRpc]
    private void RpcChagneDashState(bool state)
    {
        _isDashing = state;
    }

    private IEnumerator DiactivateDashingRoutine()
    {
        _canDash = false;

        yield return new WaitForSeconds(_dashReloadTime);

        _canDash = true;
    }
}