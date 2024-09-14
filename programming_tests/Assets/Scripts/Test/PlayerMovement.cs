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

    Vector2 moveDirection = Vector2.zero;

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

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        moveDirection.Normalize();
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}