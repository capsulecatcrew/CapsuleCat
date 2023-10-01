using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Avatar : MonoBehaviour
{
    public Image backgroundImg, characterImg;
    public Sprite[] bgSprites, charSprites;

    private int _currentBgSprite, _currentCharSprite = 0;
    private bool _isCurrentlySwitching = false;
    /// <summary>
    /// sets background of avatar to given index from list of backgrounds.
    /// </summary>
    /// <param name="index">index of desired background</param>
    public void SetBackgroundSprite(int index)
    {
        if (index >= bgSprites.Length) return;
        backgroundImg.sprite = bgSprites[index];
        _currentBgSprite = index;
    }

    /// <summary>
    /// sets character iconof avatar to given index from list of backgrounds.
    /// </summary>
    /// <param name="index">index of desired character icon</param>
    public void SetCharacterSprite(int index)
    {
        if (index >= charSprites.Length) return;
        characterImg.sprite = charSprites[index];
        _currentCharSprite = index;
    }

    public void SetCharAndBgSprite(int index)
    {
        SetCharacterSprite(index);
        SetBackgroundSprite(index);
    }

    public void TempSwitchSprites(int index, float seconds)
    {
        if (_isCurrentlySwitching) return;
        StartCoroutine(TempSetSpritesForSeconds(index, seconds));
    }

    private IEnumerator TempSetSpritesForSeconds(int index, float seconds)
    {
        _isCurrentlySwitching = true;
        var tempBg = _currentBgSprite;
        var tempChar = _currentCharSprite;
        SetCharAndBgSprite(index);
        yield return new WaitForSeconds(seconds);
        SetBackgroundSprite(tempBg);
        SetCharacterSprite(tempChar);
        _isCurrentlySwitching = false;
    }
}
