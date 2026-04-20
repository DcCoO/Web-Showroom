using NaughtyAttributes;
using UnityEngine;

public class VersionController : MonoBehaviour
{
    [SerializeField] private VersionContainer _container;
    public string Test;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void Setup(VersionContainer container)
    {
        _container = container;
        
    }

    public void Apply(VersionData data)
    {
        var partsController = PartsController.Instance;
        foreach (var (part, prefab) in data.Parts)
        {
            partsController.ReplacePart(part, prefab);
        }
    }
    
    [Button]
    public void TestVersion()
    { 
        var data = _container.Versions[Test]; 
        Apply(data);
    }
}
