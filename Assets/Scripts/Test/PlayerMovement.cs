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
    public BoxCollider2D col_trigger;
    public Vector2 Direction_Selected = Vector2.zero;

    public Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        actionInput = new PlayerActionsInput();
        rb = GetComponent<Rigidbody2D>();
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

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        moveDirection = FourDirections(moveDirection);

        moveDirection.Normalize();
        //uses the normalized direction of the input to define the position of the collider relative to the player. 
        //Since it's normalized, it will change at maximum 1 unit of pixel perfect (8, in this case)
        //I can change it later to balance the range of the melee attack.
        if (moveDirection != Vector2.zero)
        {
            //stores the direction selected to use on magic
            Direction_Selected = moveDirection;
            col_trigger.offset = Direction_Selected;
        }
        
    }
    //as I will probably have to get this vector in other scripts, I just made it acessible by function.

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    //function to convert the movement into 4 directions
    private Vector2 FourDirections(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Snap to horizontal direction
            return new Vector2(Mathf.Sign(direction.x), 0);
        }
        else
        {
            // Snap to vertical direction
            return new Vector2(0, Mathf.Sign(direction.y));
        }
    }
}