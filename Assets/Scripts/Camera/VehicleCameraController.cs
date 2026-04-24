using Unity.Cinemachine;
using UnityEngine;

public class VehicleCameraController : MonoBehaviour, ICameraResettable
{
    [SerializeField] private ECameraType _cameraType;

    [Header("Common References")]
    [SerializeField] private Transform _target;
    [field: SerializeField] public Transform CameraTransform { get; set; }
    [field: SerializeField] public CinemachineCamera Camera { get; set; }

    [Header("Position Mode")]
    [SerializeField] private bool _useLocalAnchorPosition;
    [SerializeField] private Transform _anchorReference;
    [SerializeField] private Vector3 _anchorLocalPosition = new Vector3(0f, 1.2f, 0.3f);

    [Header("Rotation")]
    [SerializeField] private float _horizontalAngle;
    [SerializeField] private float _verticalAngle = 30f;
    [SerializeField] private float _horizontalSensitivity = 3f;
    [SerializeField] private float _verticalSensitivity = 3f;
    [SerializeField] private float _minVerticalAngle = -60f;
    [SerializeField] private float _maxVerticalAngle = 85f;
    
    [Header("Exterior Only")]
    [SerializeField] private AnimationCurve _minVerticalAngleByZoom = AnimationCurve.Linear(0f, -60f, 1f, -60f);

    [Header("Zoom")]
    [SerializeField] private float _zoomValue = 10f;
    [SerializeField] private float _minZoomValue = 2f;
    [SerializeField] private float _maxZoomValue = 20f;
    [SerializeField] private float _zoomSensitivity = 5f;
    [SerializeField] private bool _zoomControlsFieldOfView;

    [Header("Smoothing")]
    [SerializeField] private float _smoothness = 8f;

    [field: SerializeField] public Vector3 OriginalPosition { get; set; }
    [field: SerializeField] public Quaternion OriginalRotation { get; set; }
    [field: SerializeField] public float OriginalFOV { get; set; }

    private float _currentHorizontalAngle;
    private float _currentVerticalAngle;
    private float _currentZoomValue;

    private void Awake()
    {
        _currentHorizontalAngle = _horizontalAngle;
        _currentVerticalAngle = _verticalAngle;
        _currentZoomValue = _zoomValue;

        if (CameraTransform != null)
        {
            OriginalPosition = CameraTransform.position;
            OriginalRotation = CameraTransform.rotation;
        }

        if (Camera != null)
        {
            OriginalFOV = Camera.Lens.FieldOfView;
        }
    }

    private void Update()
    {
        if (!IsCurrentCameraActive())
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _horizontalAngle += Input.GetAxis("Mouse X") * _horizontalSensitivity;
            _verticalAngle -= Input.GetAxis("Mouse Y") * _verticalSensitivity;
        }

        var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (!Mathf.Approximately(scrollWheel, 0f))
        {
            _zoomValue -= scrollWheel * _zoomSensitivity;
        }
    }

    private void LateUpdate()
    {
        ClampInput();
        SmoothState();
        ApplyCamera();
    }

    private bool IsCurrentCameraActive()
    {
        return CameraController.Instance != null &&
               CameraController.Instance.CurrentCameraType == _cameraType;
    }

    private void ClampInput()
    {
        _zoomValue = Mathf.Clamp(_zoomValue, _minZoomValue, _maxZoomValue);
        _verticalAngle = Mathf.Clamp(_verticalAngle, GetMinVerticalAngle(), _maxVerticalAngle);
    }

    private float GetMinVerticalAngle()
    {
        if (_useLocalAnchorPosition || _maxZoomValue <= _minZoomValue)
        {
            return _minVerticalAngle;
        }

        var zoomRatio = Mathf.InverseLerp(_minZoomValue, _maxZoomValue, _zoomValue);
        return _minVerticalAngleByZoom.Evaluate(zoomRatio);
    }

    private void SmoothState()
    {
        var t = Mathf.Clamp01(Time.deltaTime * _smoothness);
        _currentHorizontalAngle = Mathf.LerpAngle(_currentHorizontalAngle, _horizontalAngle, t);
        _currentVerticalAngle = Mathf.LerpAngle(_currentVerticalAngle, _verticalAngle, t);
        _currentZoomValue = Mathf.Lerp(_currentZoomValue, _zoomValue, t);
    }

    private void ApplyCamera()
    {
        if (CameraTransform == null)
        {
            return;
        }

        if (_useLocalAnchorPosition)
        {
            ApplyInteriorCamera();
            return;
        }

        ApplyExteriorCamera();
    }

    private void ApplyExteriorCamera()
    {
        if (_target == null)
        {
            return;
        }

        var horizontalRadians = _currentHorizontalAngle * Mathf.Deg2Rad;
        var verticalRadians = _currentVerticalAngle * Mathf.Deg2Rad;

        var offset = new Vector3(
            _currentZoomValue * Mathf.Cos(verticalRadians) * Mathf.Sin(horizontalRadians),
            _currentZoomValue * Mathf.Sin(verticalRadians),
            _currentZoomValue * Mathf.Cos(verticalRadians) * Mathf.Cos(horizontalRadians)
        );

        CameraTransform.position = _target.position + offset;
        CameraTransform.LookAt(_target);
    }

    private void ApplyInteriorCamera()
    {
        if (_anchorReference == null)
        {
            return;
        }

        CameraTransform.position = _anchorReference.TransformPoint(_anchorLocalPosition);
        CameraTransform.rotation = _anchorReference.rotation *
                                   Quaternion.Euler(_currentVerticalAngle, _currentHorizontalAngle, 0f);

        if (_zoomControlsFieldOfView && Camera != null)
        {
            Camera.Lens.FieldOfView = _currentZoomValue;
        }
    }
}
