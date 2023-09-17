using UnityEngine;

/// <summary>
/// Data structure that associates a name with an audioClip,
/// letting globalAudio find clips by name
/// 
/// TODO: these changes are not implemented as they are not of priority
/// TODO: perhaps inherit from audioSource? so we don't have to place 2 components for 1 function
/// TODO: (idea 2) have a pool of gameObjects with Sounds
/// 
/// If you need the audio to play from a specific location, just move the gameobject to that location.
/// If you need the audio to move while it's playing, then move the gameobject.
/// If you need the audio to follow another game object, just set it's parent to the other gameobject, and attach and detach as needed.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 256)] public int priority = 128;
    
    [Range(0f, 1f)] public float volume = 1;
    
    [Range(-3f, 3f)] public float pitch = 1;

    public bool loop;

    [HideInInspector] public AudioSource source;

    public void BindAudioSourceProperties(AudioSource auSource)
    {
        source = auSource;
        auSource.clip = clip;
        auSource.priority = priority;
        auSource.volume = volume;
        auSource.pitch = pitch;
        auSource.loop = loop;
    }
}
