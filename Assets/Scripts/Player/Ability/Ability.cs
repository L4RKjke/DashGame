using Mirror;

public abstract class Ability : NetworkBehaviour
{
    public AbilityStatus Status { get; protected set; }

    public abstract void ActivateAbiltity();

    public abstract void DeactivateAbiltity();
}

public enum AbilityStatus
{
    InProgress,
    Ended
}
