using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public bool resetPlayerStatsOnLoad = false;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
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
