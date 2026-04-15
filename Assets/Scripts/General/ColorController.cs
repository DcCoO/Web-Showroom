using NaughtyAttributes;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private ColorData _colorData;
    public string ColorName;
    private CarPart[] _parts;
    private bool _isCached;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    [Button]
    private void SetColor()
    {
        var material = _colorData.GetMaterial(ColorName);
        if (!_isCached)
        {
            _parts = FindObjectsByType<CarPart>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            _isCached = true;
        }

        foreach (var part in _parts)
        {
            part.ChangeBodyColor(material);
        }
    }
    
}
