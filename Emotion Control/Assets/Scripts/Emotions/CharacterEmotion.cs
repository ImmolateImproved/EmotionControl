using UnityEngine;
using UnityEngine.UI;

public class CharacterEmotion : MonoBehaviour
{
    [SerializeField] private TilemapHolder tilemapHolder;
    [SerializeField] private SpellBase currentEmotion;

    [SerializeField] private Image currentEmotionIcon;

    public void Pickup(Vector3Int node)
    {
        var key = new TilemapKey(node, 1);

        if (!tilemapHolder.Tilemap.TryGetTile<EmotionHolder>(key, out var emotionHolder)) return;

        currentEmotion = emotionHolder.Emotion;
        currentEmotionIcon.enabled = true;
        currentEmotionIcon.sprite = currentEmotion.icon;

        Destroy(emotionHolder.gameObject);
    }

    public bool Use(GridMovement movement, Vector3Int node)
    {
        if (currentEmotion == null) return false;

        return currentEmotion.Use(movement, tilemapHolder, node);
    }
}