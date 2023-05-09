using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _cameraObj;
    [SerializeField] private Transform target;
    [Header("Settings")]
    [SerializeField] private float distance = 3f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private float minYAngle = 40f;
    [SerializeField] private float maxYAngle = 80f;

     private float currentX = 0f;
     private float currentY = 0f;

    private const string _moveScrollWheel = "Mouse ScrollWheel";

    public void MoveCamera(Vector2 inputRotation, float sensetivity)
    {
        currentX += inputRotation.x * sensetivity;
        currentY -= inputRotation.y * sensetivity;

        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        distance = Mathf.Clamp(distance - Input.GetAxis(_moveScrollWheel) * sensetivity, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 position = target.position + rotation * direction;

        _cameraObj.position = position;
        _cameraObj.LookAt(target.position);
    }
}
