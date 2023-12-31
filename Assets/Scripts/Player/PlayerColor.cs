using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    [Range(1, 2)] public int playerNo = 1;
    public SpriteRenderer[] sprites;
    public GameObject[] gameObjects;
    public Image[] uiImages;
    [SerializeField] private Color _color;

    void Start()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        _color = PlayerColors.GetPlayerColor(playerNo);

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = _color;
        }
        
        foreach (GameObject obj in gameObjects)
        {
            Renderer renderer;
            if (obj.TryGetComponent(out renderer))
            {
                renderer.material.color = _color;
                if (renderer.material.IsKeywordEnabled("_EMISSION"))
                {
                    renderer.material.SetColor("_EmissionColor", _color * 0.9f);
                }
            }
            obj.GetComponent<Renderer>().material.color = _color;
        }

        foreach (Image img in uiImages)
        {
            img.color = _color;
        }

    }
}