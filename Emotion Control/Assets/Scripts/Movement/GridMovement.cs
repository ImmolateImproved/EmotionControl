using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class GridMovement
{
    private TilemapHolder tilemapHolder;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    [SerializeField] private Ease ease;

    public CharacterAnimation CharacterAnimation { get; private set; }

    public event Action OnNodeReached;

    public Vector3Int CurrentNode { get; set; }
    public Vector3Int TargetNode { get; set; }

    public Vector3Int Direction { get; private set; }

    private Tween movementTween;

    public void Init(TilemapHolder tilemapHolder, GameObject owner)
    {
        this.tilemapHolder = tilemapHolder;
        rb = owner.GetComponent<Rigidbody>();
        CharacterAnimation = owner.GetComponent<CharacterAnimation>();

        InitCharacterGridPosition(owner.transform.forward);
    }

    public void PauseMovement(float delay)
    {
        movementTween.Kill();

        rb.DOMove(rb.position, delay)
            .OnComplete(() =>
            {
                OnNodeReached?.Invoke();
            });
    }

    public Vector3Int GetNextNode(int range = 1)
    {
        var nextNode = CurrentNode + Direction * range;
        nextNode.z = 0;

        return nextNode;
    }

    public Vector3 GetNodePosition(Vector3Int node)
    {
        var position = tilemapHolder.GetNodeCenterWorld(node);
        position.y = rb.position.y;

        return position;
    }

    public void SetDirection(Vector3Int direction)
    {
        Direction = direction;

        var tmpDirection = new Vector3Int(Direction.x, 0, Direction.y);
        var rotation = Quaternion.LookRotation(tmpDirection);
        rb.DORotate(rotation.eulerAngles, 0.5f);
    }

    public bool CheckGround()
    {
        if (!tilemapHolder.CheckGround(CurrentNode))
        {
            Fall();
            return false;
        }

        return true;
    }

    public bool CheckObstacle()
    {
        return tilemapHolder.CheckObstacle(TargetNode);
    }

    public void Move()
    {
        CharacterAnimation.SetRunState(true);

        var position = GetNodePosition(TargetNode);

        movementTween = rb.DOMove(position, moveSpeed)
                .SetEase(ease)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    CurrentNode = TargetNode;
                    OnNodeReached?.Invoke();
                });
    }

    public void Jump()
    {
        movementTween.Kill();

        CurrentNode = GetNextNode(2);
        var position = GetNodePosition(CurrentNode);

        rb.DOJump(position, jumpHeight, 1, 0.5f).OnComplete(() =>
        {
            OnNodeReached?.Invoke();
        });
    }

    public void Blink()
    {
        movementTween.Kill();

        CurrentNode = GetNextNode(2);
        var position = GetNodePosition(CurrentNode);

        rb.DOMove(position, 0).OnComplete(() =>
        {
            OnNodeReached?.Invoke();
        });
    }

    public void Fall()
    {
        movementTween.Kill();
        rb.useGravity = true;
    }

    public void SetTargetNode()
    {
        CheckDirectionSign();
        TargetNode = GetNextNode();
    }

    private void CheckDirectionSign()
    {
        if (tilemapHolder.TryGetDirectionSign(CurrentNode, out var directionSign, out var key))
        {
            SetDirection(directionSign.Direction);

            tilemapHolder.Tilemap.RemoveTile(key);
            GameObject.Destroy(directionSign.gameObject);
        }
    }

    private void InitCharacterGridPosition(Vector3 forward)
    {
        CurrentNode = tilemapHolder.WorldToNode(rb.position);
        rb.position = GetNodePosition(CurrentNode);

        Direction = new Vector3Int((int)forward.x, (int)forward.z);

        TargetNode = GetNextNode();
    }
}