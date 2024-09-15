using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    public PlayerActionsInput actionInput;
    public InputAction magic_;
    public Projectile_Test magic;

    private void Awake()
    {
        actionInput = new PlayerActionsInput();
    }
    private void OnEnable()
    {
        magic_ = actionInput.Player.Magic;
        magic_.Enable();
        magic_.performed += OnMagicPerformed;
    }
    private void OnDisable()
    {
        magic_.performed -= OnMagicPerformed;
        magic_.Disable();
    }
    private void OnMagicPerformed(InputAction.CallbackContext context)
    {
        Instantiate(magic, transform.position, transform.rotation);
        Debug.Log("Magic performed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("enemy detected");
        }
    }
}
