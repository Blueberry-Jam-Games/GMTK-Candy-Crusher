using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public SoundItem titleScreen;
    public SoundItem gameplay;

    public static BGMManager Instance;

    private void Awake()
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

        titleScreen.source = gameObject.AddComponent<AudioSource>();
        titleScreen.source.clip = titleScreen.clip;
        titleScreen.source.volume = titleScreen.volume;
        titleScreen.source.outputAudioMixerGroup = titleScreen.mixerGroup;

        gameplay.source = gameObject.AddComponent<AudioSource>();
        gameplay.source.clip = gameplay.clip;
        gameplay.source.volume = gameplay.volume;
        gameplay.source.outputAudioMixerGroup = gameplay.mixerGroup;
    }

    private void Start()
    {
        titleScreen.source.Play();
    }

    public int state = 0; // 0 = title, 1 = transition, 2 = gameplay

    public void PlayGameplay()
    {
        StartCoroutine(ToggleMusic(true));
    }

    public void PlayTitle()
    {
        StartCoroutine(ToggleMusic(false));
    }

    private bool playFast = false;
    private bool transitionComplete = true;
    private readonly int transitionTime = 60;

    private IEnumerator ToggleMusic(bool fast)
    {
        if (transitionComplete && fast != playFast)
        {
            Debug.Log($"Starting transition of music from {playFast} to {fast}");
            playFast = fast;
            transitionComplete = false;

            for (float volumeTransition = 0; volumeTransition < transitionTime; volumeTransition++)
            {
                Debug.Log($"Pitch adjusting loop {volumeTransition}");
                if (playFast)
                {
                    titleScreen.volume = Mathf.MoveTowards(titleScreen.volume, 0f, 0.16f);
                    gameplay.volume = Mathf.MoveTowards(gameplay.volume, 1f, 0.16f);
                }
                else
                {
                    titleScreen.volume = Mathf.MoveTowards(titleScreen.volume, 1f, 0.16f);
                    gameplay.volume = Mathf.MoveTowards(gameplay.volume, 0f, 0.16f);
                }

                yield return new WaitForSeconds(0.15f);
            }
            Debug.Log("Transition done");
            transitionComplete = true;
        }
        yield break;
    }
}
