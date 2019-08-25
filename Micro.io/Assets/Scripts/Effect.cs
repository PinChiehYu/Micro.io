using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    public EffectType Type;
    public bool IsPositive;
    public float Amount;
    public float RemainTiming;
}

public enum EffectType
{
    MoveSpeed = 0,
    Score = 1,
    ReloadSpeed = 2,
    ShootDistance = 3
}