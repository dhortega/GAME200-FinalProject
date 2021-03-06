using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else {
            //Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            return;
        }
            

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.Log("Cannot find " + s.name + ". Was this a typo?");
            return;
        }
        if (s.source == null) {
            Debug.Log(s.name + " is null for some reason");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name, float fadeTime = 0.0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Cannot find " + s.name + ". Was this a typo?");
            return;
        }
        if (fadeTime == 0)
            s.source.Stop();
        else {
            StartCoroutine(AudioFadeOut(s.source, Math.Abs(fadeTime)));
        }
    }

    public bool AudioSourceIsPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Cannot find " + s.name + ". Was this a typo?");
            return true;
        }
        if (s.source == null)
        {
            Debug.Log(s.name + " is null for some reason");
            return true;
        }

        return s.source.isPlaying;
    }

    private IEnumerator AudioFadeOut(AudioSource source, float fadeTime) {
        float initVol = source.volume;

        while (source.volume > 0) {
            source.volume -= initVol * (Time.deltaTime / fadeTime);
            yield return null;
        }

        source.Stop();
        source.volume = initVol;
    }
}
