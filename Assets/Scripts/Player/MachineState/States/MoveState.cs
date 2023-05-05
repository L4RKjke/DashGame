using UnityEngine;

public class MoveState : State
{
    [SerializeField] private InputSystem _inputSystem;

    private void Update()
    {
        Player?.PlayerMover.Move(_inputSystem.MoveDirection);
    }
}
