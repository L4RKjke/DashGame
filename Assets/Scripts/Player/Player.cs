using System.Collections;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dashDistance;

    private float _speed = 6;
    private bool _isDashing = false;
    private float _dashSpeed = 50;
    private Coroutine _dashCoroutine;
    private float _timeOfInvulnerability = 3;

    public Action<HealthStatus> HealthChanged;
    public Action Dashed;

    public HealthStatus State { get; private set; }

    public bool IsDashing => _isDashing;

    private void Start()
    {
        State = HealthStatus.Cured;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
    }

    private void OnDisable()
    {
        if (_dashCoroutine != null)
            StopCoroutine(_dashCoroutine);
    }

    public void ApplyDamage()
    {
        StartCoroutine(ApplyDamageRoutine());
    }

    public void DashActivate()
    {
        if (_isDashing == false)
        {
            DashAnimator();
            _isDashing = true;
            var target = transform.position + (transform.forward * _dashDistance);
            _dashCoroutine = StartCoroutine(DashRoutine(target));
        }
    }

    private void DashAnimator()
    {
        _animator.SetBool("Dash", true);
    }

    public void RotateView(Vector2 rotation, float sensetivity)
    {
        var X = rotation.x * sensetivity * Time.deltaTime;
        var Y = rotation.y * (sensetivity) * Time.deltaTime;
        var eulerX = (transform.rotation.eulerAngles.x + Y) % 360;
        var eulerY = (transform.rotation.eulerAngles.y + X) % 360;

        transform.localRotation = Quaternion.Euler(transform.localRotation.x, eulerY, 0);
        _camera.Rotate(new Vector3(eulerX, 0, 0));
    }

    public void Move(Vector3 direction)
    {
        if (_isDashing == false)
        {
            direction = direction.normalized;
            Vector3 velocity = direction * _speed;
            transform.Translate(velocity * Time.deltaTime);
            _animator.SetFloat("Speed", Vector3.ClampMagnitude(direction, 1).magnitude * _speed);
        }
    }

    private IEnumerator DashRoutine(Vector3 target)
    {
        while (true)
        {
            if (transform.position != target)
            {
                Vector3 newPosition = Vector3.MoveTowards(transform.position, target, _dashSpeed * Time.deltaTime);
                UpdatePosition(newPosition);
            }
            else
            {
                _isDashing = false;
                StopDashing();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void StopDashing()
    {
        _animator.SetBool("Dash", false);
    }

    private void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    private IEnumerator ApplyDamageRoutine()
    {
        State = HealthStatus.Damaged;
        HealthChanged?.Invoke(State);

        yield return new WaitForSeconds(_timeOfInvulnerability);

        State = HealthStatus.Cured;
        HealthChanged?.Invoke(State);
    }
}

public enum HealthStatus
{
    Damaged,
    Cured
}