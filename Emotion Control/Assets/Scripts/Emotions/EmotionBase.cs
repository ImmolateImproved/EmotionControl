using UnityEngine;
using UnityEngine.UIElements;

public abstract class EmotionBase : ScriptableObject
{
    public Sprite icon;

    [SerializeField] protected ParticleSystem onUseEffect;
    [SerializeField] protected float effectDuration;
    [SerializeField] protected float useDuration;

    public abstract bool Use(GridMovement movement, TilemapHolder gridData, Vector3Int node);

    public virtual void SpawnEffect(Vector3 position)
    {
        if (onUseEffect == null) return;

        var effect = Instantiate(onUseEffect, position, Quaternion.identity);
        Destroy(effect.gameObject, effectDuration);
    }
}
