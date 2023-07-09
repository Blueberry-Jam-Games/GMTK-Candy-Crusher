using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TowerAudio : MonoBehaviour
{
    public static TowerAudio Instance;
    public SoundItem[] sounds;

    Dictionary<string, SoundItem> soundCache = new Dictionary<string, SoundItem>();

    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach(SoundItem sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.outputAudioMixerGroup = sound.mixerGroup;

            soundCache.Add(sound.name, sound);
        }
    }

    public void Play(string name)
    {
        if(soundCache.TryGetValue(name, out SoundItem sound))
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogError($"Sound {name} not found");
        }
    }
}

[System.Serializable]
public class SoundItem
{
    public string name;

    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;
}