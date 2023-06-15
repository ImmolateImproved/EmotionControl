using System.Collections;
using UnityEngine;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemapHolder;
    [SerializeField] private LevelLoader levelLoader;

    [SerializeField] private string levelGridName;

    private Grid levelGrid;
    private Character character;

    private void Awake()
    {
        character = Character.Singleton;
        levelGrid = GameObject.Find(levelGridName).GetComponent<Grid>();

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