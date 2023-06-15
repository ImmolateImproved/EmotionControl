using System.Collections;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemapHolder;
    [SerializeField] private LevelLoader levelLoader;

    [SerializeField] private Grid levelGrid;
    [SerializeField] private Character character;

    private void Start()//Start - because its need to processed after all singletons initialize
    {
        FindAllDependency();

        tilemapHolder.Init(levelGrid);
        character.Init(tilemapHolder);
        levelLoader.Init(tilemapHolder, character);

        StartCoroutine(WaitToStart());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelLoader.RestartLevel();
        }
    }

    [ContextMenu("FindAllDependency")]
    private void FindAllDependency()
    {
        character = Character.Singleton;
        levelLoader = LevelLoader.Singleton;
        levelGrid = LevelGridSingleton.Singleton.GetComponent<Grid>();
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

        character.MainLoop();
    }
}