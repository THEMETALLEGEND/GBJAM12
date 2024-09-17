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

	//vector2 to use on attacks
	public Vector2 Direction_Selected;

	Vector2 moveDirection = Vector2.zero;

	// Variables to store the last movement direction
	private float lastDirX = 0f;
	private float lastDirY = 0f;

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

			animator.SetFloat("Horizontal", moveDirection.x);
			animator.SetFloat("Vertical", moveDirection.y);

			// Save the last movement direction if the player is moving
			if (moveDirection.magnitude > 0.01f) // If there is movement
			{
				lastDirX = moveDirection.x;
				lastDirY = moveDirection.y;
			}

			// Send the last direction to the Animator 
			animator.SetFloat("lastDirX", lastDirX);
			animator.SetFloat("lastDirY", lastDirY);
		}
		else
		{
			// If movement is not allowed, reset moveDirection
			moveDirection = Vector2.zero;
		}
        if (moveDirection != Vector2.zero)
        {
            //stores the direction selected to use on magic
            Direction_Selected = moveDirection;
            
        }
    }

	private void FixedUpdate()
	{
		if (isAllowedToMove)
		{
			// Update the velocity when movement is allowed
			rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
		}
		else
		{
			// Stop the player when movement is not allowed
			rb.velocity = Vector2.zero;
		}

		// Update animator speed parameter based on velocity
		float speed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);
		animator.SetFloat("Speed", speed);
	}
}