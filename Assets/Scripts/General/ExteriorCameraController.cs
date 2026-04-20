using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineMixingCamera _mixingCamera;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _externalCamera;

    [SerializeField] private float _horizontalAngle;
    [SerializeField] private float _verticalAngle = 30f;
    [SerializeField] private AnimationCurve _minVerticalAngle;
    [SerializeField] private float _maxVerticalAngle = 85f;

    [SerializeField] private float _distance = 10f;
    [SerializeField] private float _minDistance = 2f;
    [SerializeField] private float _maxDistance = 20f;

    [SerializeField] private float _horizontalSensitivity = 3f;
    [SerializeField] private float _verticalSensitivity = 3f;
    [SerializeField] private float _zoomSensitivity = 5f;
    [SerializeField] private float _smoothness = 5f;

    private float _currentHorizontalAngle;
    private float _currentVerticalAngle;
    private float _currentDistance;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _horizontalAngle += Input.GetAxis("Mouse X") * _horizontalSensitivity;
            _verticalAngle -= Input.GetAxis("Mouse Y") * _verticalSensitivity;
        }

        _distance -= Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
    }

    private void LateUpdate()
    {
        var distanceRatio = (_distance - _minDistance) / (_maxDistance - _minDistance);
        var minVerticalAngle = _minVerticalAngle.Evaluate(distanceRatio);
        _verticalAngle = Mathf.Clamp(_verticalAngle, minVerticalAngle, _maxVerticalAngle);
        _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);

        var t = Time.deltaTime * _smoothness;
        _currentHorizontalAngle = Mathf.LerpAngle(_currentHorizontalAngle, _horizontalAngle, t);
        _currentVerticalAngle = Mathf.Lerp(_currentVerticalAngle, _verticalAngle, t);
        _currentDistance = Mathf.Lerp(_currentDistance, _distance, t);

        var hRad = _currentHorizontalAngle * Mathf.Deg2Rad;
        var vRad = _currentVerticalAngle * Mathf.Deg2Rad;

        var offset = new Vector3(
            _currentDistance * Mathf.Cos(vRad) * Mathf.Sin(hRad),
            _currentDistance * Mathf.Sin(vRad),
            _currentDistance * Mathf.Cos(vRad) * Mathf.Cos(hRad)
        );

        _externalCamera.position = _target.position + offset;
        _externalCamera.LookAt(_target);
    }
}
