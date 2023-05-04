using System.Collections;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _timeOfInvulnerability;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private NetworkIdentity _identity;

    private float _speed = 8;
    private bool _isCollision = false;
    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashSpeed = 40;
    private Coroutine _dashCoroutine;

    private readonly int _dashReloadTime = 1;
    private readonly int _maxAngle = 360;

    public Action<HealthStatus> HealthChanged;
    public Action<float> SpeedChanged;
    public Action<bool> DashStateChanged;

    public NetworkIdentity Identity => _identity;

    private HealthStatus _state = HealthStatus.Cured;

    public HealthStatus State => _state;

    public bool IsDashing => _isDashing;

    private void Start()
    {
        _state = HealthStatus.Cured;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Wall>(out Wall _))
        {
            _isCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<Wall>(out Wall _))
        {
            _isCollision = false;
        }
    }

    private void OnDisable()
    {
        if (_dashCoroutine != null)
            StopCoroutine(_dashCoroutine);
    }

    public void ApplyDamage()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(ApplyDamageRoutine());
        }
    }

    public void DashActivate()
    {
        if (_isDashing == true) return;

        if (_canDash == false) return;

        StartCoroutine(DiactivateDashingRoutine());
        _isDashing = true;

        if (isLocalPlayer)
        {
            CmdChangeDashState(true);
        }

        var target = transform.position + (transform.forward * _dashDistance);

        if (isLocalPlayer)
            _dashCoroutine = StartCoroutine(DashRoutine(target));
    }

    [Command]
    private void CmdChangeDashState(bool state)
    {
        _isDashing = state;
        RpcChagneDashState(state);
    }

    [ClientRpc]
    private void RpcChagneDashState(bool state)
    {
        _isDashing = state;
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
            _rigidbody.velocity = Vector3.zero;
            direction = direction.normalized;
            Vector3 velocity = direction * _speed;
            transform.Translate(velocity * Time.deltaTime);
            var speed = Vector3.ClampMagnitude(direction, 1).magnitude * _speed;
            SpeedChanged?.Invoke(Vector3.ClampMagnitude(direction, 1).magnitude * _speed);
        }
    }

    private IEnumerator DashRoutine(Vector3 target)
    {
        _rigidbody.velocity = transform.forward * _dashSpeed;
        DashStateChanged.Invoke(_isDashing);

        while (true)
        {
            if (Vector3.Distance(transform.position, target) < 2f || _isCollision)
            {
                _rigidbody.velocity = Vector3.zero;

                _isDashing = false;
                if (isLocalPlayer)
                    CmdChangeDashState(false);

                DashStateChanged.Invoke(_isDashing);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator ApplyDamageRoutine()
    {
        _state = HealthStatus.Damaged;

        if (isLocalPlayer)
        {
            _state = HealthStatus.Damaged;
            CmdChangeState(HealthStatus.Damaged);
        }

        yield return new WaitForSeconds(_timeOfInvulnerability);

        _state = HealthStatus.Cured;

        if (isLocalPlayer)
        {
            _state = HealthStatus.Cured;
            CmdChangeState(HealthStatus.Cured);
        }

    }

    [Command]
    private void CmdChangeState(HealthStatus state)
    {
        _state = state;
        HealthChanged?.Invoke(state);
        RpcChangeState(state);
    }

    [ClientRpc]
    private void RpcChangeState(HealthStatus status)
    {
        _state = status;
        HealthChanged?.Invoke(status);
    }

    private IEnumerator DiactivateDashingRoutine()
    {
        _canDash = false;

        yield return new WaitForSeconds(_dashReloadTime);

        _canDash = true;
    }
}