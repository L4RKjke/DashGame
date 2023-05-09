using UnityEngine;
using Mirror;
using System;

public class InputSystem : NetworkBehaviour
{
    [SerializeField] private Player _mover;
    [SerializeField] private CameraExtention _camera;

    private int _mouseSensetivity = 220;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _mouseHorizontal = "Mouse X";
    private const string _mouseVertical = "Mouse Y";

    public float HorizontalKeyInput { get; private set; }

    public float VerticalKeyInput { get; private set; }

    public float HorizontalMouseInput { get; private set; }

    public float VerticalMouseInput { get; private set; }

    public Vector3 MoveDirection { get; private set; }
    
    public bool IsClicked { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {   
        if (!isLocalPlayer) return;

        IsClicked = false;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            IsClicked = true;
        }

        OnPlayerMove();
        OnMouseMove();
    }

    private void OnPlayerMove()
    {
        float horizontalInput = Input.GetAxis(_horizontal);
        float verticalInput = Input.GetAxis(_vertical);
        var direction = new Vector3(horizontalInput, 0f, verticalInput);

        MoveDirection = direction;
    }

    private void OnMouseMove()
    {
        var horizontalKeyInput = Input.GetAxis(_mouseHorizontal);
        var verticalKeyInput = Input.GetAxis(_mouseVertical);
        var rotation = new Vector2(horizontalKeyInput, verticalKeyInput);

        _mover.PlayerMover.RotateView(rotation, _mouseSensetivity);
        _camera.RotateCamera(rotation, _mouseSensetivity);
    }
}