using Services;
using UnityEngine;

public class LevelGrid : MonoBehaviour, IService
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }
}