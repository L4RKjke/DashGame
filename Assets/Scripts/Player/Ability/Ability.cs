using Mirror;
using System;

public abstract class Ability : NetworkBehaviour
{
    public AbilityStatus Status { get; protected set; }

    public abstract void ActivateAbiltity(Action activatedCallback = null);

    public abstract void DeactivateAbiltity();
}

public enum AbilityStatus
{
    InProgress,
    Ended
}
