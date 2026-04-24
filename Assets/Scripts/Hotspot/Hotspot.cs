using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hotspot: MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [field: SerializeField] public ECameraType TargetCameraType { get; private set; }
    private bool _isInitialized;
    
    private void Awake()
    {
        CameraController.OnCameraChanged += HandleCameraChanged;
    }

    private void OnDestroy()
    { 
        CameraController.OnCameraChanged -= HandleCameraChanged;
    }
    
    private void Update()
    {
        if (!_isInitialized) return;
        transform.rotation = _cameraTransform.rotation;
    }

    public void Setup(Transform cameraTransform)
    {
        _cameraTransform = cameraTransform;
        _isInitialized = true;
        gameObject.SetActive(CameraController.Instance.CurrentCameraType == TargetCameraType);
    }

    private void HandleCameraChanged(ECameraType newCameraType)
    {
        gameObject.SetActive(newCameraType == TargetCameraType);
    }

    private void OnMouseDown()
    {
        print("aaaaaaaaaaaa ");
    }
}