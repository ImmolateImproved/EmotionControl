using DG.Tweening;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IService
{
    private Vector3Int levelEndNode;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartLevel();
        }
    }

    public void Init(TilemapHolder tilemapHolder, Character character)
    {
        character.OnNodeReached += CheckLevelEnd;

        levelEndNode = tilemapHolder.WorldToNode(transform.position);

        var position = tilemapHolder.GetNodeCenterWorld(levelEndNode);
        position.y = 0;
        transform.position = position;
    }

    private void CheckLevelEnd(Vector3Int node)
    {
        if (node == levelEndNode)
        {
            DOTween.KillAll();
            LoadNextLevel();
        }
    }

    private void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
