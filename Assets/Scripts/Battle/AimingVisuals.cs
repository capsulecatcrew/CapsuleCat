using UnityEngine;

public class AimingVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private RaycastHit _hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            _lineRenderer?.SetPosition(1, new Vector3(0, 0, _hit.distance));
        }
        else
        {
            _lineRenderer.SetPosition(1, new Vector3(0, 0, 100));
        }
    }
}
