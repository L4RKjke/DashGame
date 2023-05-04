using UnityEngine;

public class AxisInputChecker : Transition
{
    [SerializeField] private InputSystem _inputSystem;

    private void Update()
    {
        if (_inputSystem.MoveDirection != Vector3.zero)
            NeedTransit = true;
    }
}