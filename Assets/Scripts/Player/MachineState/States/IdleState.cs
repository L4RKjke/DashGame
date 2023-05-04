using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private void OnEnable()
    {
        Debug.Log("eldeState activated");

        if (Player != null)
            Player.PlayerMover.Move(Vector3.zero);
    }
}
