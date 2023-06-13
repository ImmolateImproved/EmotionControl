using System.Collections.Generic;
using UnityEngine;

public readonly struct TilemapKey
{
    public readonly Vector3Int node;
    public readonly int level;

    public TilemapKey(Vector3Int node, int level)
    {
        this.node = node;
        this.level = level;
    }
}

public class Tilemap
{
    public const int LOWER_LAYER_KEY = 0;
    public const int UPPER_LAYER_KEY = 1;

    public Dictionary<TilemapKey, Tile> tilemap;

    public Tilemap Init(Grid grid)
    {
        var tilesCount = grid.transform.childCount;
        
        tilemap = new Dictionary<TilemapKey, Tile>(tilesCount);

        for (int i = 0; i < tilesCount; i++)
        {
            var tileTransform = grid.transform.GetChild(i);
            var tile = tileTransform.GetComponent<Tile>();

            var node = grid.WorldToCell(tileTransform.position);

            var key = new TilemapKey(node, tile.level);

            tilemap.Add(key, tile);
        }

        return this;
    }

    public void AddTile(in TilemapKey key, Tile tile)
    {
        tilemap.Add(key, tile);
    }

    public bool TryGetAnyTile(in TilemapKey key, out Tile tile)
    {
        return tilemap.TryGetValue(key, out tile);
    }

    public bool TryGetTile<T>(in TilemapKey key, out T tile) where T : Tile
    {
        TryGetAnyTile(key, out var res);

        tile = res as T;

        return tile != null;
    }

    public bool RemoveTile(in TilemapKey key)
    {
        return tilemap.Remove(key);
    }

    public bool RemoveTile<T>(in TilemapKey key) where T : Tile
    {
        if (TryGetAnyTile(key, out var _))
        {
            RemoveTile(key);
            return true;
        }

        return false;
    }

    public bool RemoveTile<T>(in TilemapKey key, out T tile) where T : Tile
    {
        if (TryGetTile(key, out tile))
        {
            RemoveTile(key);
            return true;
        }

        return false;
    }
}