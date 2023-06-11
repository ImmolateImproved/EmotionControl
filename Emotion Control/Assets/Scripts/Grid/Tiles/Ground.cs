using Unity.Mathematics;
using UnityEngine;

public class Ground : Tile
{
    public Vector3Int Node { get; private set; }

    private Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void Select(bool select)
    {
        material.color = select ? Color.red : Color.white;
    }

    public void Init(Vector3Int node)
    {
        Node = node;
    }
}
