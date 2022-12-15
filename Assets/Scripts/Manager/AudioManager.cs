using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource bgmSource;


    private void Awake()
    {
        if (Instance != null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void InitAudio()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
    }


    public void PlayBGMByName(string name, bool isLoop = true)
    {
       
        //  Load the clip
        AudioClip clip = Resources.Load<AudioClip>("Audios/BGM/" + name);
        if (clip == null)
        {
            Debug.Log("[AudioManager]: is null");
        }
        else
        {
            Debug.Log("[AudioManager]: not null");
        }
        
        //  Set it to be the BGM
        bgmSource.clip = clip;
        bgmSource.loop = isLoop;
        bgmSource.Play();
    }

    public void PlayEffectByName(string name, bool isLoop = false)
    {
        //  Load the clip
        AudioClip clip = Resources.Load<AudioClip>("Audios/EffectAudio/" + name);
        //  Play the Clip
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
