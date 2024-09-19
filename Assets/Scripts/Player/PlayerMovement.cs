using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public PlayerActionsInput actionInput;
    public InputAction move;
    private Animator animator;
    [HideInInspector] public bool isAllowedToMove = true;

    private bool isOnKnockBack;

    // Vector2 to use on attacks
    public Vector2 Direction_Selected;

    Vector2 moveDirection = Vector2.zero;

    // Variables to store the last movement direction
    private float lastDirX = 0f;
    private float lastDirY = 0f;

    // Track if the magic button is being held down
    public bool isMagicButtonHeld = false; // Set this from the PlayerAttacking script

    private void Awake()
    {
        actionInput = new PlayerActionsInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        move = actionInput.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAllowedToMove)
        {
            moveDirection = move.ReadValue<Vector2>();

            // Check if magic is not being held
            if (!isMagicButtonHeld)
            {
                // Update the direction and animation facing when not casting magic
                if (moveDirection.magnitude > 0.01f) // If there is movement
                {
                    lastDirX = moveDirection.x;
                    lastDirY = moveDirection.y;

                    // Update the animation based on the direction of movement
                    animator.SetFloat("Horizontal", moveDirection.x);
                    animator.SetFloat("Vertical", moveDirection.y);

                    // Update the direction selected for magic casting
                    Direction_Selected = moveDirection;
                }
            }

            // Always send the last direction to the Animator, regardless of whether magic is held
            animator.SetFloat("lastDirX", lastDirX);
            animator.SetFloat("lastDirY", lastDirY);
        }
        else
        {
            // If movement is not allowed, reset moveDirection
            moveDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isAllowedToMove)
        {
            // Update the velocity when movement is allowed
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
        else if (!isOnKnockBack)
        {
            // Stop the player when movement is not allowed
            rb.velocity = Vector2.zero;
        }

        // Update animator speed parameter based on velocity
        float speed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);
        animator.SetFloat("Speed", speed);
        Transform meleeCollider = transform.Find("melee_collider");
        if (meleeCollider != null)
        {
            meleeCollider.position = (Vector2)transform.position + Direction_Selected;
        }
    }
    public void ApplyKnockback(Vector2 direction)
    {
        StartCoroutine(Knock_Back(direction));
    }
    private IEnumerator Knock_Back(Vector2 dir)
    {
        isAllowedToMove = false;
        isOnKnockBack = true; // handles those two variables to not conflict with the fixed update if
        rb.velocity = Vector2.zero;
        float knockbackForce = 1f; // how much the player will be knocked back
        rb.velocity = Vector2.zero;
        rb.velocity = dir * knockbackForce;
        yield return new WaitForSeconds(1f);
        isAllowedToMove = true;
        isOnKnockBack = false;
    }
}
