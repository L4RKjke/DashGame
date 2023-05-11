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

    public Action HealthDamaged;
    public Action HealthCured;

    public Ability Ability => _ability;

    public PlayerMover PlayerMover => _playerMover;

    public NetworkIdentity Identity => _identity;

    public PlayerInfo PlayerInfo => _playerInfo;

    private HealthStatus _state = HealthStatus.Cured;

    public HealthStatus State 
    { 
        get => _state;

        private set 
        {
            _state = value;

            if (value == HealthStatus.Damaged)
                HealthDamaged?.Invoke();
            else
                HealthCured?.Invoke();
        } 
    }

    private void Start()
    {
        State = HealthStatus.Cured;
    }


    public void ApplyDamage(Action callback)
    {
        if (State == HealthStatus.Damaged) return;

        callback();
        StartCoroutine(ApplyDamageRoutine());
    }

    private IEnumerator ApplyDamageRoutine()
    {
        State = HealthStatus.Damaged;
        if (isLocalPlayer)

            CmdUpdateState(HealthStatus.Damaged);

        yield return new WaitForSeconds(_timeOfInvulnerability);

        State = HealthStatus.Cured;

        if (isLocalPlayer)
            CmdUpdateState(HealthStatus.Cured);
    }

    [Command(requiresAuthority = false)]
    private void CmdUpdateState(HealthStatus state)
    {
        Debug.Log("yes12");
        State = state;
        RpcUpdateState(state);
    }

    [ClientRpc]
    private void RpcUpdateState(HealthStatus status)
    {
        State = status;
    }
}