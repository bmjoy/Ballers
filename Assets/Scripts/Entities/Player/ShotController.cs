﻿using MLAPI.NetworkedVar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    LAYUP,
    DUNK,
    SHOT
}

public enum ShotDirection
{
    FRONT,
    SIDE,
    BACK
}

public enum ShotRange
{
    CLOSE,
    LONG
}

public enum BankType
{
    NONE = -1,
    LEFT = 0,
    RIGHT = 1
}

public class ShotController : MonoBehaviour
{
    private const float CLOSE_RANGE = 8.0f;
    private const float LONGE_RANGE = 20.0f;

    /// <summary>
    /// Returns the type of shot the player should take. 
    /// </summary>
    public ShotType GetTypeOfShot(Player p, float dist, ShotDirection direction)
    {
        if (dist < CLOSE_RANGE)
        {
            if (p.isSprinting) return ShotType.DUNK;
            else if (p.isMoving) return ShotType.LAYUP;
        }

        return ShotType.SHOT;
    }

    public static ShotRange GetShotRange(ShotType type)
    {
        if (type == ShotType.SHOT)
            return ShotRange.LONG;
        return ShotRange.CLOSE;
    }

}
