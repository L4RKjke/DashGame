using Mirror;
using System;

public abstract class Ability : NetworkBehaviour
{
    public AbilityStatus Status { get; protected set; }

    public abstract void ActivateAbiltity(Action activatedCallback);

    public abstract void DeactivateAbiltity();
}

public enum AbilityStatus
{
    InProgress,
    Ended
}
