using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public Action<bool> AttackEnded;

    private void OnEnable()
    {
        if (Player != null)
            Player.Ability.ActivateAbiltity();
    }

    private void OnDisable()
    {
        if (Player != null)
            Player.Ability.DeactivateAbiltity();

        AttackEnded?.Invoke(false);
    }
}
