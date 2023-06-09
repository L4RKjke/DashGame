using UnityEngine;
using Mirror;

public class CameraExtention : NetworkBehaviour
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private Transform _cameraObj;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _playerCamera.SetActive(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer)
        {
            _playerCamera.SetActive(false);
        }
    }
}