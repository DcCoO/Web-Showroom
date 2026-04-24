using System.Collections.Generic;
using UnityEngine;

public class CameraInfoProvider : MonoBehaviour
{
    public static Dictionary<ECameraType, Transform> Cameras = new();
    [SerializeField] private ECameraType _cameraType;
    
    void Awake()
    {
        Cameras[_cameraType] = transform;
    }
    
    void Update()
    {
        
    }
}
