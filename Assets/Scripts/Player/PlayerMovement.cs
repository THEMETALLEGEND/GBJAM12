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

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

		float speed = Mathf.Abs(moveDirection.x * moveSpeed) + Mathf.Abs(moveDirection.y * moveSpeed);
		animator.SetFloat("Speed", speed);
	}
}