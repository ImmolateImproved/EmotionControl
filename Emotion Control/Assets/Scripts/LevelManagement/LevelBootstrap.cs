using Services;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemapHolder;
    [SerializeField] private DirectionSign directionSignPrefab;

    private Character character;
    private LevelLoader levelLoader;
    private PlanningStage planingStage;

    private void Start() //Start - because its need to processed after all services initialize
    {
        var levelGrid = ServiceLocator.Get<LevelGrid>().GetComponent<Grid>();
        character = ServiceLocator.Get<Character>();
        levelLoader = ServiceLocator.Get<LevelLoader>();

        tilemapHolder.Init(levelGrid);
        character.Init(tilemapHolder);
        levelLoader.Init(tilemapHolder, character);

        planingStage = new PlanningStage(tilemapHolder, directionSignPrefab);
    }

    private void Update()
    {
        WaitToStart();
    }

    private void WaitToStart()
    {
        planingStage.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.MainLoop();
            enabled = false;
        }
    }
}