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
    [SerializeField] private PlayerInfo _playerInfo;

    public Action<HealthStatus> HealthChanged;

    public Ability Ability => _ability;

    public PlayerMover PlayerMover => _playerMover;

    public NetworkIdentity Identity => _identity;


    private HealthStatus _state = HealthStatus.Cured;

    public HealthStatus State 
    { 
        get => _state;

        private set 
        {
            _state = value;
            HealthChanged?.Invoke(_state);
        } 
    }

    private void Start()
    {
        State = HealthStatus.Cured;
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
            State = HealthStatus.Damaged;
            CmdChangeState(HealthStatus.Damaged);
        }

        yield return new WaitForSeconds(_timeOfInvulnerability);

        _state = HealthStatus.Cured;

        if (isLocalPlayer)
        {
            State = HealthStatus.Cured;
            CmdChangeState(HealthStatus.Cured);
        }

    }

    [Command]
    private void CmdChangeState(HealthStatus state)
    {
        State = state;
        RpcChangeState(state);
    }

    [ClientRpc]
    private void RpcChangeState(HealthStatus status)
    {
        if (status == HealthStatus.Damaged)
        {
            _playerInfo.AddPoint();
        }

        State = status;
    }
}