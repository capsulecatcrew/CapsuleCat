using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    private static bool _setResolution = true;
    
    public Animator transition;
    private static readonly int WipeDirection = Animator.StringToHash("WipeDirection");
    private static readonly int Start = Animator.StringToHash("Start");

    public delegate void LevelChange();

    public event LevelChange OnLevelChange;

    void Awake()
    {
        transition.SetInteger(WipeDirection, Random.Range(0, 4));
        if (_setResolution)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
            _setResolution = false;
        }
        PlayerStats.SetLevelLoader(this);
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
        transition.SetInteger(WipeDirection, Random.Range(0, 4));
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(sceneName);
        OnLevelChange?.Invoke();
    }
}
