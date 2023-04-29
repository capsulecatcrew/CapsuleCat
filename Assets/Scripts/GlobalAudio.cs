using System;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio Singleton;

    public Sound[] music;
    private Sound _currentMusic;
    
    public Sound[] sounds;
    
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
            s.BindAudioSourceProperties(gameObject.AddComponent<AudioSource>());
        }
        
        foreach (Sound s in sounds)
        {
            s.BindAudioSourceProperties(gameObject.AddComponent<AudioSource>());
        }

    }

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
