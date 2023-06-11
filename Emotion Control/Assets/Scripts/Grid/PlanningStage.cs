using System;
using UnityEngine;

public class PlanningStage : MonoBehaviour
{
    [SerializeField] private TilemapHolder grid;

    [SerializeField] private DirectionSign directionSignPrefab;

    private DirectionSign lastDirectionSign;

    public static event Action<Vector3Int, bool> OnSingSpawnAndDestroy;
    public static event Action<DirectionSign> OnSingChangeDirection;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            enabled = false;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out var distance))
        {
            var point = ray.GetPoint(distance);
            var node = grid.WorldToNode(point);

            if (Input.GetMouseButtonDown(0))
            {
                SpawnDirectionSign(node);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RemoveDirectionSign(node);
            }
        }

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

    public void SpawnDirectionSign(Vector3Int node)
    {
        var key1 = new TilemapKey(node, 1);
        var key0 = new TilemapKey(node, 0);

        if (grid.Tilemap.TryGetTile<DirectionSign>(key1, out var _)) return;
        if (!grid.Tilemap.TryGetTile(key0, out var _)) return;

        OnSingSpawnAndDestroy?.Invoke(node, true);

        var position = grid.GetNodeCenterWorld(node);
        position.y = 0;

        var directionSign = Instantiate(directionSignPrefab, position, Quaternion.identity);
        lastDirectionSign = directionSign;

        grid.Tilemap.AddTile(key1, directionSign);
    }

    public void RemoveDirectionSign(Vector3Int node)
    {
        var key = new TilemapKey(node, 1);

        if (!grid.Tilemap.RemoveTile<DirectionSign>(key, out var directionSign)) return;

        Destroy(directionSign.gameObject);

        OnSingSpawnAndDestroy?.Invoke(node, false);
    }
}