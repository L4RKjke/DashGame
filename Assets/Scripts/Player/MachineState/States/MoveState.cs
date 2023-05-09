using System;
using UnityEngine;

public class MoveState : State
{
    [SerializeField] private InputSystem _inputSystem;

    public Action SpeedChanged;

    private void FixedUpdate()
    {
        Player?.PlayerMover.Move(_inputSystem.MoveDirection);

        SpeedChanged?.Invoke();
    }
}
