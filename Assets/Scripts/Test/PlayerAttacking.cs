using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    public PlayerActionsInput actionInput; 
    [HideInInspector]  public InputAction magic_;
    [HideInInspector]  public InputAction melee_attack;
    public GameObject magic; // the projectile that will be launched when X pressed
    public GameObject meleeCollider; // the collider that detects enemies in the melee attack
    private bool isAttacking = false;
    [HideInInspector] public Vector2 magicDirection = Vector2.zero; // direction to the magic follow when created
    private bool isMagicCharging = false; // checks if the button is being held
    public int hp = 10;
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
        if (!isMagicCharging)
        {
            // captures the player direction to use on the magic but on the moment it started holding button
            magicDirection = transform.right;
            isMagicCharging = true;
            Debug.Log("Magic is being hold");
        }
    }

    private void OnMagicCanceled(InputAction.CallbackContext context)
    {
        if (isMagicCharging)
        {
            if (magicDirection == Vector2.zero) // this checks if the player didnt move yet. if so, it dont cast magics because it doesnt know the intended direction.
            {                                   //but we can change that if you want.
                Debug.Log("Player didn't submit any direction inputs yet so it won't cast the magic");
            }
            else
            {
                GameObject magicProjectile = Instantiate(magic, transform.position, transform.rotation);
                magicProjectile.SetActive(true);
                Projectile_Test projectileScript = magicProjectile.GetComponent<Projectile_Test>();
                if (projectileScript != null)
                {
                    projectileScript.GetDirection(magicDirection); 
                }
                isMagicCharging = false;
                Debug.Log("Magic casted");
            }
        }
    }


    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        { // uses overlapcircleall to check all the enemies and damage all of them at the same time
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, EnemyLayer);
            foreach (Collider2D collider in hitColliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Damage(1);
                }
            }

            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("Player took damage");
        hp -= dmg;
        if (hp <= 0)
            Debug.Log("Player died");
    }
}
