using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    public PlayerActionsInput actionInput;
    public InputAction magic_;
    public InputAction melee_attack;
    public GameObject magic;
    public GameObject meleeCollider;
    private bool isAttacking = false;
    private Vector2 magicDirection;
    private bool isMagicCharging = false;
    public int hp;
    public LayerMask EnemyLayer;

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
        magic_.started += OnMagicStarted;
        magic_.canceled += OnMagicCanceled;
    }

    private void OnDisable()
    {
        melee_attack.performed -= OnMeleePerformed;
        melee_attack.Disable();
        magic_.started -= OnMagicStarted;
        magic_.canceled -= OnMagicCanceled;
        magic_.Disable();
    }

    private void OnMagicStarted(InputAction.CallbackContext context)
    {
        if (isMagicCharging == false)
        {
            magicDirection = transform.right;
            Debug.Log("Magic is being hold");
            isMagicCharging = true;
        }
    }

    private void OnMagicCanceled(InputAction.CallbackContext context)
    {
        if (isMagicCharging)
        {
            GameObject magicProjectile = Instantiate(magic, transform.position, transform.rotation);
            magicProjectile.SetActive(true);
            Projectile_Test projectileScript = magicProjectile.GetComponent<Projectile_Test>();
            projectileScript.GetDirection(magicDirection);
            isMagicCharging = false;
            Debug.Log("Magic casted");
        }
    }

    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, EnemyLayer);
        foreach (Collider2D collider in hitColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(1); // Certifique-se de que o método TakeDamage_Enemy não exige parâmetros
            }
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }


    public void TakeDamage(int dmg)
    {
        hp -= dmg;
    }
}