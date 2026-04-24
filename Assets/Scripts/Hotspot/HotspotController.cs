using System.Collections.Generic;
using UnityEngine;

public class HotspotController : MonoBehaviour
{
    [SerializeField] private List<Hotspot> _hotspots;
    
    
    void Start()
    {
        foreach (var hotspot in _hotspots)
        {
            hotspot.Setup(CameraInfoProvider.Cameras[hotspot.TargetCameraType]);
        }
    }

    void Update()
    {
        
    }
}
