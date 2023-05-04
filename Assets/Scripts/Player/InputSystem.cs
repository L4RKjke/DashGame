using UnityEngine;
using Mirror;

public class InputSystem : NetworkBehaviour
{
    [SerializeField] private Player _mover;

    private int _mouseSensetivity = 220;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _mouseHorizontal = "Mouse X";
    private const string _mouseVertical = "Mouse Y";

    public float HorizontalKeyInput { get; private set; }

    public float VerticalKeyInput { get; private set; }

    public float HorizontalMouseInput { get; private set; }

    public float VerticalMouseInput { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {   
        if (Input.GetKeyUp(KeyCode.K))
        {
            if (isLocalPlayer)
                CmdTest();
        }

        if (!isLocalPlayer) return;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseDown();
        }

        OnPlayerMove();
        OnMouseMove();
    }

    [ClientRpc]
    private void RpcTest()
    {
        Debug.Log("K pressed from server to client");
    }

    [Command]
    private void CmdTest()
    {
        Debug.Log("K pressed on server");
        RpcTest();
    }

    private void OnPlayerMove()
    {
        float horizontalInput = Input.GetAxis(_horizontal);
        float verticalInput = Input.GetAxis(_vertical);
        var direction = new Vector3(horizontalInput, 0f, verticalInput);

        _mover.Move(direction);
    }

    public void OnLeftMouseDown()
    {
        _mover.DashActivate();
    }

    public void OnMouseMove()
    {
        var horizontalKeyInput = Input.GetAxis(_mouseHorizontal);
        var verticalKeyInput = -Input.GetAxis(_mouseVertical);
        var rotation = new Vector2(horizontalKeyInput, verticalKeyInput);

        _mover.RotateView(rotation, _mouseSensetivity);
    }
}