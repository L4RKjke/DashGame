using UnityEngine;
using System;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (_player.IsDashing) return;

            if (player.State != HealthStatus.Cured) return;

            if (player.IsDashing == true) return;
 
            player.ApplyDamage();
            _player.Dashed?.Invoke();
        }
    }
}
