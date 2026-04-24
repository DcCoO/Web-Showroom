using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;


public class CameraController : SingletonMonoBehaviour<CameraController>
{
    public static event Action<ECameraType> OnCameraChanged;
    [field: SerializeField] public ECameraType  CurrentCameraType { get; private set; }
    [SerializeField] private CinemachineMixingCamera _mixingCamera;
    [SerializeField] private SerializedDictionary<ECameraType, int> _cameraTypeToIndex;
    private bool _isAnimating;
    public ECameraType testCameraType; //todo:: REMOVER DEPOIS

    private void Start()
    {
        foreach (var (_, cameraTransform) in CameraInfoProvider.Cameras)
        {
            cameraTransform.GetComponent<ICameraResettable>().Set();
        }
    }
    
    public void ChangeCameraType(ECameraType cameraType)
    {
        if (cameraType == CurrentCameraType) return;
        if (_isAnimating) return;
        _isAnimating = true;
        var newCameraTransform = CameraInfoProvider.Cameras[cameraType];
        newCameraTransform.GetComponent<ICameraResettable>().Reset();
        StartCoroutine(ChangeCameraCoroutine(CurrentCameraType, cameraType));
    }
    
    private IEnumerator ChangeCameraCoroutine(ECameraType oldCameraType, ECameraType newCameraType)
    {
        var oldIndex = _cameraTypeToIndex[oldCameraType];
        var newIndex = _cameraTypeToIndex[newCameraType];

        for (var t = 0f; t < 1f; t += Time.deltaTime)
        {
            _mixingCamera.SetWeight(oldIndex, 1 - t);
            _mixingCamera.SetWeight(newIndex, t);
            yield return null;
        }
        _mixingCamera.SetWeight(oldIndex, 0);
        _mixingCamera.SetWeight(newIndex, 1);
        CurrentCameraType = newCameraType;
        OnCameraChanged?.Invoke(newCameraType);
        _isAnimating = false;
    }

    [Button]
    private void TestChangedCamera()
    {
        ChangeCameraType(testCameraType);
    }
}
