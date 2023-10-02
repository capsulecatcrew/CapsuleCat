using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailConfig : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.GetComponent<TrailRenderer>().Clear();
    }
}
