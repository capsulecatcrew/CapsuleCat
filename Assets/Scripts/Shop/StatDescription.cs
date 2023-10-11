using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDescription : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        titleText.text = "";
        descriptionText.text = "";
    }

    public void UpdateDescription()
    {
        
    }
    
}
