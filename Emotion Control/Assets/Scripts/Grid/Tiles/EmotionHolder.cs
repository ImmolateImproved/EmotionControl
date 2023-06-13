using UnityEngine;

public class EmotionHolder : Tile
{
    [field: SerializeField] public EmotionBase Emotion { get; private set; }
}