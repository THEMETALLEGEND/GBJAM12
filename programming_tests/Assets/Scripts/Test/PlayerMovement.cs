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
        moveDirection.Normalize();
        //uses the normalized direction of the input to define the position of the collider relative to the player. 
        //Since it's normalized, it will change at maximum 1 unit of pixel perfect (8, in this case)
        //I can change it later to balance the range of the melee attack.
        if (moveDirection != Vector2.zero)
        {
            col_trigger.offset = moveDirection;
        }
        
    }
    //as I will probably have to get this vector in other scripts, I just made it acessible by function.

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}