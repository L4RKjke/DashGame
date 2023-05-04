using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerMover : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;

    private readonly int _maxAngle = 360;
    public Action<float> SpeedChanged;

    public void RotateView(Vector2 rotation, float sensetivity)
    {
        var X = rotation.x * sensetivity * Time.deltaTime;
        var eulerY = (transform.rotation.eulerAngles.y + X) % _maxAngle;

        transform.localRotation = Quaternion.Euler(transform.localRotation.x, eulerY, 0);
    }

    public void Move(Vector3 direction)
    {
        direction = direction.normalized;
        Vector3 velocity = direction * _speed;
        transform.Translate(velocity * Time.deltaTime);
        _rigidbody.velocity = Vector3.zero;
        var speed = Vector3.ClampMagnitude(direction, 1).magnitude * _speed;
        SpeedChanged?.Invoke(speed);
    }
}
