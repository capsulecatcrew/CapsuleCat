using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio Singleton;

    public Sound[] music;
    private Sound _currentMusic;
    
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }

    }

    // Update is called once per frame
    public void PlaySound(string soundName)
    {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Global Audio: Sound " + soundName + " not found!");
            return;
        }
        sound.source.Play();
    }
    
    public void StopSound(string soundName)
    {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Global Audio: Sound " + soundName + " not found!");
            return;
        }
        sound.source.Stop();
    }    
    public void PlayMusic(string songName)
    {
        Sound sound = Array.Find(music, s => s.name == songName);
        if (sound == null)
        {
            Debug.LogWarning("Global Audio: Music " + songName + " not found!");
            return;
        }

        _currentMusic = sound;
        sound.source.Play();
    }
    
    public void StopMusic(string songName)
    {
        Sound sound = Array.Find(music, s => s.name == songName);
        if (sound == null)
        {
            Debug.LogWarning("Global Audio: Music " + songName + " not found!");
            return;
        }
        sound.source.Stop();
    }
    
    public void StopMusic()
    {
        _currentMusic?.source.Stop();
    }


}
