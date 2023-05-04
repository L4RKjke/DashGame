using UnityEngine;
using Mirror;

public class MachineState : NetworkBehaviour
{
    [SerializeField] private State _startState;
    [SerializeField] private Player _player;

    private State _currentState;

    public State CurrentState => _currentState;

    private void Start()
    {
        Reset(_startState);
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetState();

        if (nextState != null)
            NextState(nextState);
    }


    public void NextState(State newState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = newState;

        if (_currentState != null)
            _currentState.Enter(_player);
    }

    private void Reset(State startState)
    {
        _currentState = startState;
        _currentState.Enter(_player);
    }
}