using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        var r = Random.Range(0.1f, 1.0f);
        var g = Random.Range(0.1f, 1.0f);
        var b = Random.Range(0.1f, 1.0f);

        foreach (var renderer in renderers)
        {
            renderer.material.color = new Color(r, g, b);
        }
    }

}
