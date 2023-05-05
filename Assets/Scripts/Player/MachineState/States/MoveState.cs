using UnityEngine;

public class MoveState : State
{
    [SerializeField] private InputSystem _inputSystem;

    private void Update()
    {
        if (Player != null)
            Player.PlayerMover.Move(_inputSystem.MoveDirection);
    }
}
