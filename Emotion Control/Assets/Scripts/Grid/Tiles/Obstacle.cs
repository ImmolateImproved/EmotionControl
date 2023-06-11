using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Stone, Trap
}

public class Obstacle : Tile
{
    [field: SerializeField] public ObstacleType ObstacleType { get; private set; }
}
