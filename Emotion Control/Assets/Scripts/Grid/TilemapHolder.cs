using DG.Tweening;
using UnityEngine;

public class TilemapHolder : MonoBehaviour
{
    [SerializeField] private Grid levelGrid;
    [SerializeField] private LevelLoader levelLoader;

    public Tilemap Tilemap { get; private set; }

    private Vector3Int levelEndNode;

    private void Awake()
    {
        Init();
        InitLevelLoader();
    }

    public void Init()
    {
        Tilemap = new Tilemap().Init(levelGrid);
    }

    public void InitLevelLoader()
    {
        var node = WorldToNode(levelLoader.transform.position);
        levelEndNode = node;

        var position = GetNodeCenterWorld(node);
        position.y = 0;
        levelLoader.transform.position = position;
    }

    public void CheckLevelEnd(Vector3Int node)
    {
        if (node == levelEndNode)
        {
            DOTween.KillAll();
            levelLoader.LoadNextLevel();
        }
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