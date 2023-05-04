using UnityEngine;

public class LeftMouseChecker : Transition
{
    [SerializeField] private InputSystem _inputSystem;

    private void Update()
    {
        if (_inputSystem.IsClicked)
            NeedTransit = true;
    }
}