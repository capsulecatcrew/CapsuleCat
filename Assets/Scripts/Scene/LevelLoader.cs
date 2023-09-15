using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour
{
    private static bool _setResolution = true;
    
    public Animator transition;
    private static readonly int WipeAnimator = Animator.StringToHash("WipeDirection");
    private static readonly int StartAnimator = Animator.StringToHash("Start");

    public delegate void LevelChange(string sceneName);

    public event LevelChange OnLevelChange;

    public void Awake()
    {
        transition.SetInteger(WipeAnimator, Random.Range(0, 4));
        if (!_setResolution) return;
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
        _setResolution = false;
    }

    public void Start()
    {
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

    private IEnumerator SceneTransition(string sceneName)
    {
        transition.SetInteger(WipeAnimator, Random.Range(0, 4));
        transition.SetTrigger(StartAnimator);
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(sceneName);
        OnLevelChange?.Invoke(sceneName);
    }
}
