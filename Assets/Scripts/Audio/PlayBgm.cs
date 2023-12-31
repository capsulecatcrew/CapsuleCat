// Author:  Ian Hong  (ianfromdover@gmail.com)

using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Exposes the song ID to editor
/// to edit in the scene, and plays them on awake
///
/// Not used in Battle scene as randomisation is needed
/// </summary>
public class PlayBgm : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string songId;
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (playOnStart) StartBgm();
    }

    /// <summary>
    /// Can be called through events
    /// </summary>
    public void StartBgm()
    {
        if (songId == "")
        {
            Debug.LogError("PlayBgm: No song ID provided, unable to find BGM.");
            return;
        }

        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic(songId);
    }
}
