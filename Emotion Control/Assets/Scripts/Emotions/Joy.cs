using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Emorions/Joy")]
public class Joy : SpellBase
{
    public override bool Use(GridMovement movement, TilemapHolder gridData, Vector3Int node)
    {
        var key = new TilemapKey(node, 0);

        if (!gridData.Tilemap.TryGetTile(key, out var tile))
        {
            var position = movement.GetNodePosition(movement.CurrentNode);

            SpawnEffect(position);

            movement.Jump();

            return true;
        }

        return false;
    }
}
