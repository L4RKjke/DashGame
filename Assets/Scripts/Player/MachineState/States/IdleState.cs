using System;
using UnityEngine;

public class IdleState : State
{
    public Action IdleStateActivated;

    private void OnEnable()
    {
        if (Player == null) return;

        Player.PlayerMover.Move(Vector3.zero);

        IdleStateActivated?.Invoke();
    }

    private void Update()
    {
        Player.PlayerMover.StopPlayer();
    }
}
