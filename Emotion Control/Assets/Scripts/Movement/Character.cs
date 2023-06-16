using DG.Tweening;
using Services;
using System;
using UnityEngine;

public class Character : MonoBehaviour, IService
{
    [SerializeField] private GridMovement movement;
    [SerializeField] private CharacterEmotion characterEmotion;

    public event Action<Vector3Int> OnNodeReached;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    private void OnEnable()
    {
        movement.OnNodeReached += MainLoop;
    }

    private void OnDisable()
    {
        movement.OnNodeReached -= MainLoop;
    }

    public void Init(TilemapHolder tilemapHolder)
    {
        movement.Init(tilemapHolder, gameObject);
        characterEmotion.Init(tilemapHolder);
    }

    public void MainLoop()
    {
        OnNodeReached?.Invoke(movement.CurrentNode);

        if (!movement.CheckGround()) return;
        movement.SetTargetNode();

        TryPickupEmotion();
        if (TryUseEmotion()) return;

        movement.CheckObstacle();
        movement.Move();
    }

    private void TryPickupEmotion()
    {
        characterEmotion.TryPickup(movement.CurrentNode);
    }

    private bool TryUseEmotion()
    {
        return characterEmotion.TryUse(movement, movement.TargetNode);
    }
}