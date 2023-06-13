using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterEmotion
{
    private TilemapHolder tilemapHolder;
    private EmotionBase currentEmotion;
    [SerializeField] private Image currentEmotionIcon;

    public void Init(TilemapHolder tilemapHolder)
    {
        this.tilemapHolder = tilemapHolder;
    }

    public void TryPickup(Vector3Int node)
    {
        var key = new TilemapKey(node, Tilemap.UPPER_LAYER_KEY);

        if (!tilemapHolder.Tilemap.TryGetTile<EmotionHolder>(key, out var emotionHolder)) return;

        currentEmotion = emotionHolder.Emotion;

        Object.Destroy(emotionHolder.gameObject);

        if (currentEmotionIcon)
        {
            currentEmotionIcon.enabled = true;
            currentEmotionIcon.sprite = currentEmotion.icon;
        }
    }

    public bool TryUse(GridMovement movement, Vector3Int node)
    {
        if (currentEmotion == null) return false;

        return currentEmotion.Use(movement, tilemapHolder, node);
    }
}