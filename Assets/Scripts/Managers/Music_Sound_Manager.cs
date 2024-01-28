using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Sound_Manager : MonoBehaviour
{
    [Header("音乐播放器")]
    public AudioSource musicAudioSource;
    public AudioSource musicAudioSource_Drift;
    public AudioSource musicAudioSource_Strom;
    [Header("普通音效播放器")]
    public AudioSource SFXsource;
    [Header("创音效播放器")]
    public AudioSource Cracksource;
    [Header("创音效s")]
    public List<AudioClip> crackSounds;
    [Header("点击音效")]
    public AudioClip clickSound;


    [Header("音乐s")]
    public AudioClip defaultMusic;
    public AudioClip driftMusic;
    public AudioClip powerMusic;


    
    [Header("特殊音效s")]
    public List<AudioClip> specialSounds;

    public static Music_Sound_Manager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void CreateCrackSound()
    {
        int index = Random.Range(0, crackSounds.Count);
        Cracksource.PlayOneShot(crackSounds[index]);
    }

    public void PlayDriftMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource_Drift.Stop();
        musicAudioSource_Strom.Stop();
        //musicAudioSource.clip = driftMusic;
        musicAudioSource_Drift.Play();
    }

    public void PlayPowerMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource_Drift.Stop();
        musicAudioSource_Strom.Stop();
        //musicAudioSource.clip = powerMusic;
        musicAudioSource_Strom.Play();
    }
    public void PlayNormalMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource_Drift.Stop();
        musicAudioSource_Strom.Stop();
        //musicAudioSource.clip = defaultMusic;
        musicAudioSource.Play();
    }
    public void PlayPowerSound()
    {
        SFXsource.PlayOneShot(specialSounds[0]);
    }
    public void TestSound()
    {
        SFXsource.PlayOneShot(clickSound);
    }
}
