using System.Collections;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineMixingCamera _mixingCamera;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    [Button]
    private void test()
    {
        StartCoroutine(testRoutine());
    }

    private IEnumerator testRoutine()
    {
        var p0 = _mixingCamera.Weight0;
        var p1 = _mixingCamera.Weight1;
        var p0f = 1 - p0;
        var p1f = 1 - p1;
        
        for(var t = 0f; t < 1; t += Time.deltaTime)
        {
            _mixingCamera.Weight0 = Mathf.Lerp(p0, p0f, t);
            _mixingCamera.Weight1 = Mathf.Lerp(p1, p1f, t);
            yield return null;
        }
        _mixingCamera.Weight0 = p0f;
        _mixingCamera.Weight1 = p1f;
    }
}
