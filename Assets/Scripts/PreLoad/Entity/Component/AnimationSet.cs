using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
public class AnimationSet : ScriptableObject
{
    public List<Pair<String, AnimationData>> animations;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Custom/AnimationSet")]
    public static void CreateAnimationData()
    {
        ScriptableObjectUtility.CreateAsset<AnimationSet>("AnimationSet");
    }
#endif
}