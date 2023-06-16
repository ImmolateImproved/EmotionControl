using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PrefabPalette")]
public class PrefabPalette : ScriptableObject
{
    public GameObject[] prefabs;
    public int level;
    public float y;

    public int Count => prefabs.Length;
}