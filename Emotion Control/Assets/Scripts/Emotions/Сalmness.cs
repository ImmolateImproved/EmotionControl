using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Emorions/Сalmness")]
public class Сalmness : EmotionBase
{
    public override bool Use(GridMovement movement, TilemapHolder gridData, Vector3Int node)
    {
        var key = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);

        if (gridData.Tilemap.TryGetTile<Obstacle>(key, out var obstacle))
        {
            if (obstacle.ObstacleType != ObstacleType.Trap) return false;

            var position = gridData.GetNodeCenterWorld(movement.CurrentNode);
            SpawnEffect(position);

            movement.Blink();

            position = gridData.GetNodeCenterWorld(movement.CurrentNode);
            SpawnEffect(position);

            return true;
        }

        return false;
    }
}