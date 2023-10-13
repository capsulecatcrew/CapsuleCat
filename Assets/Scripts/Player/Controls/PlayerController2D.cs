using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController2D : MonoBehaviour
    {
        [Range(1, 2)]
        public int playerNo = 1;
        public float moveSpeed = 5;
        public Vector2 interactBoxSize = new Vector2(1, 1);
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        private Rigidbody2D _rb;
        private float _xVelocity;

        void Start()
        {
            // Assign rigidbody for physics calculations
            _rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            // _rb.velocity = new Vector2(_xVelocity * moveSpeed * Time.fixedDeltaTime, 0);
            _rb.MovePosition(_rb.position + new Vector2(_xVelocity * moveSpeed * Time.fixedDeltaTime, 0));
        }

        void OnMoveLR(InputValue val)
        {
            _xVelocity = val.Get<float>();
            animator.SetFloat("Velocity", Mathf.Abs(_xVelocity));
            if (_xVelocity != 0) spriteRenderer.flipX = _xVelocity < 0;
        }

        void OnPrimary()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(_rb.position, interactBoxSize, 0, Vector2.zero);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
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
}
