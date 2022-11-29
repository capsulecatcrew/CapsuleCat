using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    private static bool setResolution = true;
    
    public Animator transition;

    public bool resetPlayerStatsOnLoad = false;
    void Awake()
    {
        transition.SetInteger("WipeDirection", Random.Range(0, 4));
        if (setResolution)
        {
            // Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
            setResolution = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    IEnumerator SceneTransition(string sceneName)
    {
        transition.SetInteger("WipeDirection", Random.Range(0, 4));
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.6f);
        if (resetPlayerStatsOnLoad) PlayerStats.ResetStats();
        SceneManager.LoadScene(sceneName);
    }
}
