using UnityEngine;
using Unity.Cinemachine;

public class InteriorCameraController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform _interiorCamera;
    [SerializeField] private CinemachineCamera _cinemachineCamera; // substituiu o Camera component
    [SerializeField] private Transform _carTransform;

    [Header("Posição do Anchor (local ao carro)")]
    [SerializeField] private Vector3 _anchorLocalPosition = new Vector3(0f, 1.2f, 0.3f);

    [Header("Rotação Horizontal")]
    [SerializeField] private float _horizontalSensitivity = 3f;

    [Header("Rotação Vertical")]
    [SerializeField] private float _verticalSensitivity = 3f;
    [SerializeField] private float _minVerticalAngle = -60f;
    [SerializeField] private float _maxVerticalAngle = 60f;

    [Header("Zoom (Field of View)")]
    [SerializeField] private float _fov = 60f;
    [SerializeField] private float _minFov = 20f;
    [SerializeField] private float _maxFov = 90f;
    [SerializeField] private float _zoomSensitivity = 10f;

    [Header("Suavização")]
    [SerializeField] private float _smoothness = 8f;

    private float _horizontalAngle = 0f;
    private float _verticalAngle = 0f;
    private float _currentHorizontalAngle = 0f;
    private float _currentVerticalAngle = 0f;
    private float _currentFov;

    private void Start()
    {
        _currentFov = _fov;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _horizontalAngle += Input.GetAxis("Mouse X") * _horizontalSensitivity;
            _verticalAngle -= Input.GetAxis("Mouse Y") * _verticalSensitivity;
        }

        _verticalAngle = Mathf.Clamp(_verticalAngle, _minVerticalAngle, _maxVerticalAngle);

        _fov -= Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
        _fov = Mathf.Clamp(_fov, _minFov, _maxFov);
    }

    private void LateUpdate()
    {
        var t = Time.deltaTime * _smoothness;
        _currentHorizontalAngle = Mathf.LerpAngle(_currentHorizontalAngle, _horizontalAngle, t);
        _currentVerticalAngle = Mathf.LerpAngle(_currentVerticalAngle, _verticalAngle, t);
        _currentFov = Mathf.Lerp(_currentFov, _fov, t);

        var worldAnchorPosition = _carTransform.TransformPoint(_anchorLocalPosition);
        _interiorCamera.position = worldAnchorPosition;

        var rotation = _carTransform.rotation *
                       Quaternion.Euler(_currentVerticalAngle, _currentHorizontalAngle, 0f);
        _interiorCamera.rotation = rotation;

        // Altera o FOV direto no Cinemachine, não no Camera component
        _cinemachineCamera.Lens.FieldOfView = _currentFov;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_carTransform == null) return;

        var worldPos = _carTransform.TransformPoint(_anchorLocalPosition);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(worldPos, 0.05f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(worldPos, _carTransform.forward * 0.3f);
    }
#endif
}