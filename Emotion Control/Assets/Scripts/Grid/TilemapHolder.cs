using Services;
using UnityEngine;

public class TilemapHolder : MonoBehaviour
{
    private Grid levelGrid;

    public Tilemap Tilemap { get; private set; }

    public void Init(Grid levelGrid)
    {
        this.levelGrid = levelGrid;
        Tilemap = new Tilemap().Init(levelGrid);
    }

    public bool CheckGround(Vector3Int node)
    {
        var currentNodeKey0 = new TilemapKey(node, Tilemap.LOWER_LAYER_KEY);

        return Tilemap.TryGetTile<Ground>(currentNodeKey0, out var _);
    }

    public bool CheckObstacle(Vector3Int node)
    {
        var nextNodeKey1 = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);

        return Tilemap.TryGetTile<Obstacle>(nextNodeKey1, out var _);
    }

    public bool TryGetDirectionSign(Vector3Int node, out DirectionSign directionSign, out TilemapKey key)
    {
        key = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);

        return Tilemap.TryGetTile<DirectionSign>(key, out directionSign);
    }

    public Vector3Int WorldToNode(Vector3 worldPosition)
    {
        return levelGrid.WorldToCell(worldPosition);
    }

    public Vector3 GetNodeCenterWorld(Vector3Int node)
    {
        return levelGrid.GetCellCenterWorld(node);
    }
}