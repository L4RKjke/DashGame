using System;

public class AttackState : State
{
    public Action<bool> AttackStateChanged;

    private void OnEnable()
    {
        if (Player == null) return;

        Player?.Ability.ActivateAbiltity(() => AttackStateChanged?.Invoke(true));
    }

    private void OnDisable()
    {
        if (Player != null)
            Player.Ability.DeactivateAbiltity();

        AttackStateChanged?.Invoke(false);
    }
}
