using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemap;

    [SerializeField] private GridMovement movement;
    [SerializeField] private CharacterEmotion characterEmotion;

    private void Awake()
    {
        movement.Init(tilemap, gameObject);
        characterEmotion.Init(tilemap);

        StartCoroutine(WaitToStart());
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

        MainLoop();
    }

    private void MainLoop()
    {
        tilemap.CheckLevelEnd(movement.CurrentNode);

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