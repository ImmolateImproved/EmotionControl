using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Emorions/Anger")]
public class Anger : SpellBase
{
    public override bool Use(GridMovement movement, TilemapHolder gridData, Vector3Int node)
    {
        var key = new TilemapKey(node, 1);

        if (gridData.Tilemap.TryGetTile<Obstacle>(key, out var obstacle))
        {
            if (obstacle.ObstacleType != ObstacleType.Stone) return false;

            movement.PauseMovement(useDuration);

            movement.characterAnimation.Attack();

            var position = gridData.GetNodeCenterWorld(node);

            SpawnEffect(position);

            gridData.Tilemap.RemoveTile(key);
            Destroy(obstacle.gameObject);

            return true;
        }

        return false;
    }
}