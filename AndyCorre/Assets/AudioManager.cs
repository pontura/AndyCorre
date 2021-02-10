using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSourceManager[] all;
    [Serializable]
    public class AudioSourceManager
    {
        public string sourceName;
        [HideInInspector] public AudioSource audioSource;
        public float volume = 1;
    }
    void Awake()
    {
        Events.PlaySound += PlaySound;
        Events.ChangeVolume += ChangeVolume;
        Events.FadeVolume += FadeVolume;       

        foreach (AudioSourceManager m in all)
        {
            m.audioSource = gameObject.AddComponent<AudioSource>();
            m.audioSource.volume = m.volume;
        }

    }
    private void OnDestroy()
    {
        Events.ChangeVolume -= ChangeVolume;
        Events.PlaySound -= PlaySound;
        Events.FadeVolume -= FadeVolume;
    }
    void ChangeVolume(string sourceName, float volume)
    {
        AudioSource audioSource = GetAudioSource(sourceName);
        audioSource.volume = volume;
    }
    void PlaySound(string sourceName, string audioName, bool loop)
    {
        AudioSource audioSource = GetAudioSource(sourceName);
        audioSource.clip = Resources.Load<AudioClip>("Audio/" + audioName) as AudioClip;
        audioSource.Play();
        audioSource.loop = loop;
    }
    AudioSource GetAudioSource(string sourceName)
    {
        foreach (AudioSourceManager m in all)
            if (m.sourceName == sourceName)
                return m.audioSource;
        return null;
    }
    void FadeVolume(string sourceName, float to, float speed)
    {
        StopAllCoroutines();
        AudioSource audioSource = GetAudioSource(sourceName);
        StartCoroutine(FadeVolToCoroutine(audioSource, to, speed));
    }
    IEnumerator FadeVolToCoroutine(AudioSource audioSource, float to, float speed)
    {        
        float vol = audioSource.volume;
        if (to > vol)
        {
            while (to >= vol)
            {
                yield return new WaitForEndOfFrame();
                vol += Time.deltaTime / speed;
                audioSource.volume = vol;
            }
        }
        else
        {
            while (to <= vol)
            {
                yield return new WaitForEndOfFrame();
                vol -= Time.deltaTime / speed;
                audioSource.volume = vol;
            }
        }     
    }
}
