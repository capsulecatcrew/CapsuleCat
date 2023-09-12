using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Avatar : MonoBehaviour
{
    public Image backgroundImg, characterImg;
    public Sprite[] bgSprites, charSprites;

    /// <summary>
    /// sets background of avatar to given index from list of backgrounds.
    /// </summary>
    /// <param name="index">index of desired background</param>
    public void SetBackgroundSprite(int index)
    {
        if (index >= bgSprites.Length) return;
        backgroundImg.sprite = bgSprites[index];
    }

    /// <summary>
    /// sets character iconof avatar to given index from list of backgrounds.
    /// </summary>
    /// <param name="index">index of desired character icon</param>
    public void SetCharacterSprite(int index)
    {
        if (index >= charSprites.Length) return;
        characterImg.sprite = charSprites[index];
    }
}
