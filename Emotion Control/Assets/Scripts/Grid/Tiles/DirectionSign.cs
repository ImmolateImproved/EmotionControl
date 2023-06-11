using UnityEngine;

public class DirectionSign : Tile
{
    public Vector3Int Direction { get; private set; }

    private void Awake()
    {
        SetDirection(transform.forward);
    }

    public void SetDirection(Vector3 forward)
    {
        transform.forward = forward;
        Direction = new Vector3Int((int)forward.x, (int)forward.z);
    }
}