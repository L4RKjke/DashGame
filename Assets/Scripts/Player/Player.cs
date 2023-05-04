using System.Collections;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _timeOfInvulnerability;
    [SerializeField] private NetworkIdentity _identity;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private Ability _ability;

    public Action<HealthStatus> HealthChanged;
    public Action<bool> DashStateChanged;

    public Ability Ability => _ability;

    public PlayerMover PlayerMover => _playerMover;

    public NetworkIdentity Identity => _identity;


    private HealthStatus _state = HealthStatus.Cured;

    public HealthStatus State => _state;

    private void Start()
    {
        _state = HealthStatus.Cured;
    }

    public void ApplyDamage()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(ApplyDamageRoutine());
        }
    }

    private IEnumerator ApplyDamageRoutine()
    {
        _state = HealthStatus.Damaged;

        if (isLocalPlayer)
        {
            _state = HealthStatus.Damaged;
            CmdChangeState(HealthStatus.Damaged);
        }

        yield return new WaitForSeconds(_timeOfInvulnerability);

        _state = HealthStatus.Cured;

        if (isLocalPlayer)
        {
            _state = HealthStatus.Cured;
            CmdChangeState(HealthStatus.Cured);
        }

    }

    [Command]
    private void CmdChangeState(HealthStatus state)
    {
        _state = state;
        HealthChanged?.Invoke(state);
        RpcChangeState(state);
    }

    [ClientRpc]
    private void RpcChangeState(HealthStatus status)
    {
        _state = status;
        HealthChanged?.Invoke(status);
    }
}