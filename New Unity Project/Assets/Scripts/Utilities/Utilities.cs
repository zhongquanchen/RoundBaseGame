using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WalkDescription
{
    Walkable,
    NotWalkable
}

public static class Utilities
{
    public const float PATH_HEIGHT_INTERVAL = .5f;

    public const float ROUND_COUNTER = 30f;

    public const int MAX_TEAM_NUM = 6;
} 