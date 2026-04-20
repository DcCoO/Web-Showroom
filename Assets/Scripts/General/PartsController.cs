using AYellowpaper.SerializedCollections;
using UnityEngine;

public class PartsController : SingletonMonoBehaviour<PartsController>
{
    [SerializeField] private SerializedDictionary<ECarPart, GameObject> _parts;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReplacePart(ECarPart part, GameObject newPart)
    {
        if (_parts.TryGetValue(part, out var currentPart))
        {
            var position = currentPart.transform.position;
            var rotation = currentPart.transform.rotation;
            var localScale = currentPart.transform.localScale;
            var parent = currentPart.transform.parent;
            
            Destroy(currentPart);
            _parts[part] = Instantiate(newPart, position, rotation, parent);
            _parts[part].transform.localScale = localScale;
        };
    }
}
