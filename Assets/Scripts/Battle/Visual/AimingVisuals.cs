using UnityEngine;

public class AimingVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layersToAimFor;
    [SerializeField] private RectTransform targetReticle;
    [SerializeField] private RectTransform renderingCanvas;

    private const float MaxDist = 100;
    private RaycastHit _hit;
    
    private Vector2 _reticleViewportPos;
    private Vector2 _reticleCanvasPos;

    public void OnEnable()
    {
        targetReticle.gameObject.SetActive(true);
    }
    
    public void OnDisable()
    {
        if (targetReticle != null) targetReticle.gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, MaxDist,layerMask:layersToAimFor))
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, _hit.distance));
            _reticleViewportPos = Camera.main.WorldToViewportPoint(_hit.point);
            targetReticle.gameObject.SetActive(true);
            var sizeDelta = renderingCanvas.sizeDelta;
            _reticleCanvasPos.x = _reticleViewportPos.x * sizeDelta.x - sizeDelta.x * 0.5f;
            _reticleCanvasPos.y = _reticleViewportPos.y * sizeDelta.y - sizeDelta.y * 0.5f;
            targetReticle.anchoredPosition = _reticleCanvasPos;
        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, MaxDist));
            targetReticle.gameObject.SetActive(false);
        }
    }
}
