using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "VersionData", menuName = "Scriptable Objects/VersionData")]
public class VersionData : ScriptableObject
{
    public SerializedDictionary<ECarPart, GameObject> Parts;
}
