using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCursor : MonoBehaviour
{
    [Range(1, 2)]
    public int playerNo = 1;
    public Color color;
    public SpriteRenderer sprite;
    public Vector2 boundarySize = new Vector2(4, 2.25f);
    public float selectRadius = 0.25f;
    public float moveSpeed = 5;
    private Vector2 _movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(_movement.x, _movement.y, 0) * moveSpeed * Time.deltaTime);
        // limit position within boundary
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -boundarySize.x, boundarySize.x),
                                         Mathf.Clamp(transform.position.y, -boundarySize.y, boundarySize.y));
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(Vector3.zero, 2 * boundarySize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, selectRadius);
    }

    void OnMove(InputValue val)
    {
        _movement = val.Get<Vector2>();
    }

    void OnPrimary()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, selectRadius);

        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.GetComponent<Interactable>())
                {
                    Interactable interactable = hit.transform.GetComponent<Interactable>();
                    interactable.Interact();
                    interactable.InteractWithInt(playerNo);
                    // break to stop at the first interactable
                    break;
                }
            }
        }

    }
}
