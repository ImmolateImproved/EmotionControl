using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Vector3Int levelEndNode;

    public void Init(TilemapHolder tilemapHolder, Character character)
    {
        character.OnNodeReached += CheckLevelEnd;

        var node = tilemapHolder.WorldToNode(transform.position);
        levelEndNode = node;

        var position = tilemapHolder.GetNodeCenterWorld(node);
        position.y = 0;
        transform.position = position;
    }

    public void CheckLevelEnd(Vector3Int node)
    {
        if (node == levelEndNode)
        {
            DOTween.KillAll();
            LoadNextLevel();
        }
    }

    public void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
