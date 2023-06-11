using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PrefabBrush")]
[CustomGridBrush(false, true, false, "Prefab Brush")]
public class PrefabBrush : GridBrush
{
    public PrefabPalette[] prefabPalettes;

    [HideInInspector] public int paletteIndex;
    [HideInInspector] public int prefabIndex;

    private TilemapHolder tilemapHolder;

    public PrefabPalette CurrentPalette => prefabPalettes[paletteIndex];

    public int PalettesCount => prefabPalettes.Length;

    public TilemapHolder TilemapHolder { get => tilemapHolder = tilemapHolder != null ? tilemapHolder : FindObjectOfType<TilemapHolder>(); }

    public int GetPrefabCount(int paletteIndex)
    {
        return prefabPalettes[paletteIndex].prefabs.Length;
    }

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int node)
    {
        var key = new TilemapKey(node, CurrentPalette.level);
        
        if (GetObjectInCell(key)) return;

        GameObject prefab = CurrentPalette.prefabs[prefabIndex];
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");

        if (instance != null)
        {
            instance.transform.SetParent(brushTarget.transform);
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(node.x, node.y, 0) + new Vector3(.5f, .5f, CurrentPalette.y)));

            TilemapHolder.Tilemap.AddTile(key, instance.GetComponent<Tile>());
        }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int node)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        node.z = 0;

        var key0 = new TilemapKey(node, 0);
        var key1 = new TilemapKey(node, 1);

        var tile0 = GetObjectInCell(key0);
        var tile1 = GetObjectInCell(key1);

        if (tile1)
        {
            Undo.DestroyObjectImmediate(tile1);
            TilemapHolder.Tilemap.RemoveTile(key1);
        }
        else if (tile0)
        {
            Undo.DestroyObjectImmediate(tile0);
            TilemapHolder.Tilemap.RemoveTile(key0);
        }
    }

    private GameObject GetObjectInCell(TilemapKey key)
    {
        if (TilemapHolder.Tilemap.TryGetTile(key, out var tile))
        {
            return tile.gameObject;
        }

        return null;
    }
}
