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

    //private void Awake()
    //{
    //    actionInput = new PlayerActionsInput();
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void OnEnable()
    //{
    //    move = actionInput.Player.Move;
    //    move.Enable();
    //}

    //private void OnDisable()
    //{
    //    move.Disable();
    //}

    void Update()
	{
		// picked up the raw axis because they are int, so it made the pixel perfect work better
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector2 movement = new Vector2(horizontal, vertical);

		//Normalize is to be sure it will ever be 0 or 1 or -1 
		movement.Normalize();
		transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;
        //old movement code below
        //moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
	{
        //old movement code below
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

    }
}
