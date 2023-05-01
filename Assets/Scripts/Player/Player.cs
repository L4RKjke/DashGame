using System.Collections;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _timeOfInvulnerability;

    private float _speed = 10;
    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashSpeed = 50;
    private Coroutine _dashCoroutine;

    private readonly int _dashReloadTime = 1;
    private readonly int _maxAngle = 360;

    public Action<HealthStatus> HealthChanged;
    public Action Dashed;
    public Action<float> SpeedChanged;
    public Action<bool> DashStateChanged;

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
        if (_isDashing == true) return;

        if (_canDash == false) return;

        StartCoroutine(DiactivateDashingRoutine());
        _isDashing = true;
        DashStateChanged.Invoke(_isDashing);
        var target = transform.position + (transform.forward * _dashDistance);
        _dashCoroutine = StartCoroutine(DashRoutine(target));
    }

    public void RotateView(Vector2 rotation, float sensetivity)
    {
        var X = rotation.x * sensetivity * Time.deltaTime;
        var Y = rotation.y * (sensetivity) * Time.deltaTime;
        var eulerX = (transform.rotation.eulerAngles.x + Y) % _maxAngle;
        var eulerY = (transform.rotation.eulerAngles.y + X) % _maxAngle;

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
            var speed = Vector3.ClampMagnitude(direction, 1).magnitude * _speed;
            SpeedChanged?.Invoke(Vector3.ClampMagnitude(direction, 1).magnitude * _speed);
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
                DashStateChanged.Invoke(_isDashing);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
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

    private IEnumerator DiactivateDashingRoutine()
    {
        _canDash = false;

        yield return new WaitForSeconds(_dashReloadTime);

        _canDash = true;
    }
}

public enum HealthStatus
{
    Damaged,
    Cured
}