using DG.Tweening;
using UnityEngine;

public class TilemapHolder : MonoBehaviour
{
    [field: SerializeField] public Grid levelGrid { get; private set; }
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

    public bool PathAvailable(in Vector3Int node)
    {
        var key0 = new TilemapKey(node, 0);
        var key1 = new TilemapKey(node, 1);

        Tilemap.TryGetTile(key1, out var res);

        var noObstacle = (res as Obstacle) == null;

        return noObstacle && Tilemap.TryGetTile(key0, out var _);
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