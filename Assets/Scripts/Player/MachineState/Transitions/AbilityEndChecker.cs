using UnityEngine;

public class AbilityEndChecker : Transition
{
    [SerializeField] private Player _player;

    private void Update()
    {
        if (_player.Ability.Status == AbilityStatus.Ended)
            NeedTransit = true;
    }
}
