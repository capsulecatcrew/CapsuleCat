using System;
using UnityEngine;

/// <summary>
/// Controls BGM and global-scale sound effects.
/// 
/// For localised sound effects in the envt,
/// Expose AudioSources and AudioClips in the respective scripts instead.
/// Eg. EnemyLaser.cs
/// </summary>
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

    // Used for 'global-scale' sound effects that are not localised
    // eg. ui button sfx
    public void PlaySound(string soundName)
    {
        Sound sound = FindSound(sounds, soundName);
        sound.source.Play();
    }
    
    public void StopSound(string soundName)
    {
        Sound sound = FindSound(sounds, soundName);
        sound.source.Stop();
    }    
    public void PlayMusic(string songName)
    {
        Sound sound = FindSound(music, songName);
        _currentMusic = sound;
        sound.source.Play();
    }
    
    public void StopMusic()
    {
        _currentMusic?.source.Stop();
    }
    
    /// <summary>
    /// For use if there are 2 songs playing and you want to
    /// stop a specific one.
    /// </summary>
    /// <param name="songName">The song you wish to stop</param>
    public void StopMusic(string songName)
    {
        Sound sound = FindSound(music, songName);
        sound.source.Stop();
    }
    
    private Sound FindSound(Sound[] arr, string name)
    {
        Sound sound = Array.Find(arr, s => s.name == name);
        if (sound != null) return sound;

        string sourceName = arr == music
                ? "Music " 
                : arr == sounds
                ? "Sound "
                : "Unknown ";
        Debug.LogWarning("Global Audio: " + sourceName + name + " not found!");
        return null;
    }
}
