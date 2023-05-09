using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class DashAbility : Ability
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Player _player;
    [Header("Dash config")]
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _timeOfInvulnerability;

    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashSpeed = 80;
    private Coroutine _dashCoroutine;
    private Coroutine _diactivateRoutine;

    private readonly int _dashReloadTime = 1;

    public Action<HealthStatus> HealthChanged;
    public Action<float> SpeedChanged;
    public Action<bool> DashStateChanged;

    public bool IsDashing
    {
        get => _isDashing;

        private set
        {
            _isDashing = value;
            DashStateChanged?.Invoke(_isDashing);
        }
    }

    private void OnEnable()
    {
        Status = AbilityStatus.Ended;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            if (player.State == HealthStatus.Damaged) return;

            if (IsDashing)
            {
                player.ApplyDamage(() => _player.PlayerInfo.AddPoint());
            }
        }
    }

    public override void ActivateAbiltity(Action activatetCallback)
    {
        if (IsDashing == true) return;

        if (_canDash == false) return;

        _diactivateRoutine = StartCoroutine(DiactivateDashingRoutine());

        IsDashing = true;

        if (isLocalPlayer)
        {
            CmdChangeDashState(true);
        }

        var target = transform.position + (transform.forward * _dashDistance);

        if (isLocalPlayer)
        {
            _dashCoroutine = StartCoroutine(DashRoutine(target));
            activatetCallback();
        }
    }

    private IEnumerator DashRoutine(Vector3 target)
    {
        _rigidbody.velocity = transform.forward * _dashSpeed;
        Status = AbilityStatus.InProgress;
        float currentDistance = 0;

        while (true)
        {
            currentDistance += _dashSpeed * Time.deltaTime;

            if (currentDistance >= _dashDistance)
            {
                _rigidbody.velocity = Vector3.zero;

                IsDashing = false;

                if (isLocalPlayer)
                    CmdChangeDashState(false);

                Status = AbilityStatus.Ended;
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

        IsDashing = false;

        if (isLocalPlayer)
            CmdChangeDashState(false);

        Status = AbilityStatus.Ended;
    }

    [Command]
    private void CmdChangeDashState(bool state)
    {
        IsDashing = state;
        RpcChagneDashState(state);
    }

    [ClientRpc]
    private void RpcChagneDashState(bool state)
    {
        IsDashing = state;
    }

    private IEnumerator DiactivateDashingRoutine()
    {
        _canDash = false;

        yield return new WaitForSeconds(_dashReloadTime);

        _canDash = true;
    }
}