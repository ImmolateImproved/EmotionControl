using Services;
using System;
using UnityEngine;

using Object = UnityEngine.Object;

public class PlanningStage
{
    private readonly TilemapHolder tilemapHolder;
    private readonly DirectionSign directionSignPrefab;

    private DirectionSign lastDirectionSign;

    public static event Action<Vector3Int, bool> OnSingSpawnAndDestroy;
    public static event Action<DirectionSign> OnSingChangeDirection;

    public PlanningStage(TilemapHolder tilemapHolder, DirectionSign directionSignPrefab)
    {
        this.tilemapHolder = tilemapHolder;
        this.directionSignPrefab = directionSignPrefab;
    }

    public void Update()
    {
        SpawnDespawnSign();

        ChangeSingDirection();
    }

    private void SpawnDespawnSign()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out var distance))
        {
            var point = ray.GetPoint(distance);
            var node = tilemapHolder.WorldToNode(point);

            if (Input.GetMouseButtonDown(0))
            {
                SpawnDirectionSign(node);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RemoveDirectionSign(node);
            }
        }
    }

    private void ChangeSingDirection()
    {
        if (!lastDirectionSign) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            lastDirectionSign.SetDirection(Vector3.forward);
            OnSingChangeDirection?.Invoke(lastDirectionSign);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastDirectionSign.SetDirection(Vector3.left);
            OnSingChangeDirection?.Invoke(lastDirectionSign);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirectionSign.SetDirection(Vector3.back);
            OnSingChangeDirection?.Invoke(lastDirectionSign);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastDirectionSign.SetDirection(Vector3.right);
            OnSingChangeDirection?.Invoke(lastDirectionSign);
        }
    }

    private void SpawnDirectionSign(Vector3Int node)
    {
        var key1 = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);
        var key0 = new TilemapKey(node, Tilemap.LOWER_LAYER_KEY);

        if (tilemapHolder.Tilemap.TryGetTile<DirectionSign>(key1, out var _)) return;
        if (!tilemapHolder.Tilemap.TryGetAnyTile(key0, out var _)) return;

        OnSingSpawnAndDestroy?.Invoke(node, true);

        var position = tilemapHolder.GetNodeCenterWorld(node);
        position.y = 0;

        var directionSign = Object.Instantiate(directionSignPrefab, position, Quaternion.identity);
        lastDirectionSign = directionSign;

        tilemapHolder.Tilemap.AddTile(key1, directionSign);
    }

    private void RemoveDirectionSign(Vector3Int node)
    {
        var key = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);

        if (!tilemapHolder.Tilemap.RemoveTile<DirectionSign>(key, out var directionSign)) return;

        Object.Destroy(directionSign.gameObject);

        OnSingSpawnAndDestroy?.Invoke(node, false);
    }
}