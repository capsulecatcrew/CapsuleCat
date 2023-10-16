using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialScriptableObject tutorialScriptableObject;
    [SerializeField] private Image imageBox;
    [SerializeField] private TMP_Text titleTextBox;
    [SerializeField] private TMP_Text descriptionTextBox;
    [SerializeField] private TMP_Text pageNumTextBox;

    public UnityEvent onFinish;
    
    private int _numOfPages;
    private int _currentPage = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _numOfPages = tutorialScriptableObject.pages.Length;
        titleTextBox.text = tutorialScriptableObject.title;
        RefreshPage();
    }

    public void MoveToNextPage()
    {
        if (_currentPage == _numOfPages - 1)
        {
            AfterLastPage();
            return;
        }
        _currentPage++;
        RefreshPage();
    }

    public void MoveToPrevPage()
    {
        if (_currentPage == 0) return;
        _currentPage--;
        RefreshPage();
    }

    private void RefreshPage()
    {
        imageBox.sprite = tutorialScriptableObject.pages[_currentPage].image;
        descriptionTextBox.text = tutorialScriptableObject.pages[_currentPage].text;
        pageNumTextBox.text = (_currentPage + 1) +  "/" + _numOfPages;
    }

    private void AfterLastPage()
    {
        onFinish?.Invoke();
    }
}
