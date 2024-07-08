using System;
using UnityEngine;
using Unity.VisualScripting;

[Serializable]
public class Sound
{
    [SerializeField]
    string name;

    [SerializeField]
    AudioClip audio;

    public string GetName() 
    {
        return name;
    }

    public AudioClip GetAudio()
    {
        return audio;
    }
}