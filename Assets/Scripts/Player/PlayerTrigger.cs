using UnityEngine;
using System;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (_player.IsDashing)
            {
                if (player.State == HealthStatus.Cured && player.IsDashing == false)
                {
                    player.ApplyDamage();
                    _player.Dashed?.Invoke();
                }
            }
        }
    }
}
