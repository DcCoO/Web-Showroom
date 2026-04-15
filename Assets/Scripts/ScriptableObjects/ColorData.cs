using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Scriptable Objects/ColorData")]
public class ColorData : ScriptableObject
{
    public SerializedDictionary<string, Material> Colors;
    
    public Material GetMaterial(string key)
    {
        return Colors.ContainsKey(key) ? Colors[key] : null;
    }
}
