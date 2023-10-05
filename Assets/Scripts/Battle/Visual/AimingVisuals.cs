using UnityEngine;

public class AimingVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask layersToAimFor;
    [SerializeField] private RectTransform targetReticle;
    [SerializeField] private RectTransform renderingCanvas;
    
    private float maxDist = 100;
    private RaycastHit _hit;
    
    private Vector2 _reticleViewportPos = new Vector2();
    private Vector2 _reticleCanvasPos = new Vector2();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        targetReticle.gameObject.SetActive(true);
    }
    
    void OnDisable()
    {
        if (targetReticle != null) targetReticle.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, maxDist,layerMask:layersToAimFor))
        {
            _lineRenderer?.SetPosition(1, new Vector3(0, 0, _hit.distance));
            _reticleViewportPos = Camera.main.WorldToViewportPoint(_hit.point);
            targetReticle.gameObject.SetActive(true);
            var sizeDelta = renderingCanvas.sizeDelta;
            _reticleCanvasPos.x = _reticleViewportPos.x * sizeDelta.x - sizeDelta.x * 0.5f;
            _reticleCanvasPos.y = _reticleViewportPos.y * sizeDelta.y - sizeDelta.y * 0.5f;
            targetReticle.anchoredPosition = _reticleCanvasPos;
        }
        else
        {
            _lineRenderer.SetPosition(1, new Vector3(0, 0, maxDist));
            targetReticle.gameObject.SetActive(false);
        }
    }
}
