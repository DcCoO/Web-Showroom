using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    public Renderer Renderer;
    public List<int> BodyColorIndices;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [Button]
    private void GetRenderer()
    {
        Renderer = GetComponent<Renderer>();
    }
    
    public void ChangeBodyColor(Material material)
    {
        var materials = Renderer.materials;
        foreach (var bodyColorIndex in BodyColorIndices)
        {
            materials[bodyColorIndex] = material;
        }
        Renderer.materials = materials;
    }
}
