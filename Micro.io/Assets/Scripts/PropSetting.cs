using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PropSetting : ScriptableObject
{
    public GameObject Prefab;
    public List<AnimationClip> PropAnimations;
    public List<Range> EffectRanges; 
}

[Serializable]
public class Range
{
    public float Min;
    public float Max;
}