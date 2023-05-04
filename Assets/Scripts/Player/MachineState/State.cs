using UnityEngine;
using Mirror;

public abstract class State : NetworkBehaviour
{
    [SerializeField] Transition[] _transitions;

    protected State CurrentState;

    protected Player Player { get; private set; }

    public void Enter(Player player)
    {
        Player = player;
        enabled = true;

        foreach (var transition in _transitions)
        {
            transition.enabled = true;
        }

    }

    public void Exit()
    {
        foreach (var transition in _transitions)
            transition.enabled = false;

        enabled = false;

    }

    public State GetState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.NeedTransit)
                return transition.TargetState;
        }

        return null;
    }

}
