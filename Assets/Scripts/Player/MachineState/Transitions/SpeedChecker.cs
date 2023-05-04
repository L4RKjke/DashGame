using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : Transition
{
    [SerializeField] private InputSystem _inputSystem;

    private void Update()
    {
        if (_inputSystem.MoveDirection == Vector3.zero)
            NeedTransit = true;
    }
}
