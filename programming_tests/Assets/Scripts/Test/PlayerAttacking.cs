using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    public PlayerActionsInput actionInput;
    public InputAction magic_;
    public InputAction melee_attack;
    public Projectile_Test magic;
    public GameObject meleeCollider;
    private bool isAttacking = false;

    void Start()
    {
        meleeCollider.SetActive(false);
    }
    private void Awake()
    {
        actionInput = new PlayerActionsInput();
    }
    private void OnEnable()
    {
        melee_attack = actionInput.Player.Melee;
        magic_ = actionInput.Player.Magic;
        melee_attack.Enable();
        magic_.Enable();
        melee_attack.performed += OnMeleePerformed;
        magic_.performed += OnMagicPerformed;
    }
    private void OnDisable()
    {
        melee_attack.performed -= OnMeleePerformed;
        melee_attack.Disable();
        magic_.performed -= OnMagicPerformed;
        magic_.Disable();
    }
    private void OnMagicPerformed(InputAction.CallbackContext context)
    {
        Instantiate(magic, transform.position, transform.rotation);
        Debug.Log("Magic performed");
    }
    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        meleeCollider.SetActive(true);
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        meleeCollider.SetActive(false);
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("enemy detected");
        }
    }
}
