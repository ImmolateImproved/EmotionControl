using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemap;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;

    [SerializeField] private Ease ease;

    [HideInInspector] public CharacterAnimation characterAnimation;

    public Vector3Int CurrentNode { get; set; }

    public Vector3Int Direction { get; private set; }

    public Vector3Int NextNode
    {
        get
        {
            var nextNode = CurrentNode + Direction;
            nextNode.z = 0;

            return nextNode;
        }
    }

    private Tween movementTween;

    private Rigidbody rb;

    private CharacterEmotion characterEmotion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterAnimation = GetComponent<CharacterAnimation>();
        characterEmotion = GetComponent<CharacterEmotion>();

        InitPosition();
        InitDirection();
    }

    private void Start()
    {
        StartCoroutine(WaitToStart());
        //Move();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void GameOver()
    {
        rb.DOKill();
    }

    public void Move()
    {
        characterAnimation.SetRunState(true);
        tilemap.CheckLevelEnd(CurrentNode);

        characterEmotion.Pickup(CurrentNode);

        var currentNodeKey1 = new TilemapKey(CurrentNode, 1);

        if (tilemap.Tilemap.TryGetTile<DirectionSign>(currentNodeKey1, out var directionSign))
        {
            SetDirection(directionSign.Direction);

            tilemap.Tilemap.RemoveTile(currentNodeKey1);
            Destroy(directionSign.gameObject);
        }

        if (characterEmotion.Use(this, NextNode)) return;

        var nextNodeKey1 = new TilemapKey(NextNode, 1);

        if (tilemap.Tilemap.TryGetTile<Obstacle>(nextNodeKey1, out var _)) return;

        var currentNodeKey0 = new TilemapKey(CurrentNode, 0);
        if (!tilemap.Tilemap.TryGetTile(currentNodeKey0, out var _))
        {
            GameOver();
            rb.useGravity = true;
            return;
        }
        //if (!grid.PathAvailable(NextNode)) return;

        var position = GetNodePosition(NextNode);

        movementTween = rb.DOMove(position, moveSpeed)
                .SetEase(ease)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    Move();
                    CurrentNode = NextNode;
                });
    }

    public void Jump()
    {
        movementTween.Kill();

        CurrentNode = NextNode;
        CurrentNode = NextNode;
        var position = GetNodePosition(CurrentNode);

        rb.DOJump(position, jumpHeight, 1, 0.5f).OnComplete(() =>
        {
            var nextNodeKey0 = new TilemapKey(NextNode, 0);

            if (!tilemap.Tilemap.TryGetTile(nextNodeKey0, out var _))
            {
                GameOver();
                rb.useGravity = true;
                return;
            }

            Move();

        });
    }

    public void Blink()
    {
        movementTween.Kill();

        CurrentNode = NextNode;
        var position = GetNodePosition(NextNode);

        rb.DOMove(position, 0).OnComplete(() =>
        {
            Move();

        });
    }

    public void PauseMovement(float delay)
    {
        movementTween.Kill();

        rb.DOMove(rb.position, delay)
            .OnComplete(() =>
          {
              Move();
          });
    }

    public Vector3 GetNodePosition(Vector3Int node)
    {
        var position = tilemap.GetNodeCenterWorld(node);
        position.y = rb.position.y;

        return position;
    }

    private IEnumerator WaitToStart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            yield return null;
        }

        Move();
    }

    private void InitPosition()
    {
        CurrentNode = tilemap.WorldToNode(rb.position);
        rb.position = GetNodePosition(CurrentNode);
    }

    private void InitDirection()
    {
        var forward = transform.forward;
        Direction = new Vector3Int((int)forward.x, (int)forward.z);
    }

    private void SetDirection(Vector3Int direction)
    {
        Direction = direction;

        var tmpDirection = new Vector3Int(Direction.x, 0, Direction.y);
        var rotation = Quaternion.LookRotation(tmpDirection);
        transform.DORotateQuaternion(rotation, 0.5f);
    }
}