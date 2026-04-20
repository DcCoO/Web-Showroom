using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "VersionContainer", menuName = "Scriptable Objects/VersionContainer")]
public class VersionContainer : ScriptableObject
{
    public SerializedDictionary<string, VersionData> Versions;
}
