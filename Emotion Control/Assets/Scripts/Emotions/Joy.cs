using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Emorions/Joy")]
public class Joy : EmotionBase
{
    public override bool Use(GridMovement movement, TilemapHolder gridData, Vector3Int node)
    {
        var key = new TilemapKey(node, Tilemap.LOWER_LAYER_KEY);

        if (!gridData.Tilemap.TryGetAnyTile(key, out var tile))
        {
            var position = movement.GetNodePosition(movement.CurrentNode);

            SpawnEffect(position);

            movement.Jump();

            return true;
        }

        return false;
    }
}
