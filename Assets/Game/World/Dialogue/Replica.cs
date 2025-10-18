using System;
using UnityEngine;

[Serializable]
public struct Replica
{
    [TextArea]
    public string[] Dialogues;
    public Sprite Icon;
    public string AnimationName;
    public AudioClip SFX;
    //[Range(0.1f, 0.2f)]
    //public float Speed;
}