using Unity.Cinemachine;
using UnityEngine;

public interface ICameraResettable
{
    [field: SerializeField] public Vector3 OriginalPosition { get; set; }
    [field: SerializeField] public Quaternion OriginalRotation { get; set; }
    [field: SerializeField] public float OriginalFOV { get; set; }
    [field: SerializeField] public Transform CameraTransform { get; set; }
    [field: SerializeField] public CinemachineCamera Camera { get; set; }
    
    

    public void Reset()
    {
        CameraTransform.position = OriginalPosition;
        CameraTransform.rotation = OriginalRotation;
        Camera.Lens.FieldOfView = OriginalFOV;
    }

    public void Set()
    {
        OriginalPosition = CameraTransform.position;
        OriginalRotation = CameraTransform.rotation;
        OriginalFOV = Camera.Lens.FieldOfView;
    }
}
