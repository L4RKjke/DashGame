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

    public void RotateCamera(Vector2 rotation, float sensetivity)
    {
        var maxAngel = 360;
        var Y = rotation.y * (sensetivity) * Time.deltaTime;
        var eulerX = (transform.rotation.eulerAngles.x + Y) % maxAngel;
        eulerX = Mathf.Clamp(eulerX, -80, 80);
        var cameraPositionY = _cameraObj.localPosition.y + (eulerX * 0.045f);
        var cameraPositionZ = _cameraObj.localPosition.z + (eulerX * 0.045f);

        _cameraObj.localPosition = new Vector3(_cameraObj.localPosition.x, cameraPositionY, cameraPositionZ);
        _cameraObj.Rotate(new Vector3(eulerX, 0, 0));
    }
}