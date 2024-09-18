using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacking : MonoBehaviour
{
    public PlayerActionsInput actionInput;
    [HideInInspector] public InputAction magic_;
    [HideInInspector] public InputAction melee_attack;
    public GameObject magic; 
    public GameObject meleeCollider; // the collider created when you attack
    private bool isAttacking = false;
    public Vector2 magicDirection; // direction of the projectile
    private bool isMagicCharging = false; 
    private bool isMagicOnCooldown = false; 
    public int hp = 10;
    public LayerMask EnemyLayer;

    public float magicCooldownTime = 0.3f; // cooldown between magics

    private Coroutine magicCoroutine; 

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
            magicDirection = gameObject.GetComponent<PlayerMovement>().Direction_Selected;
            isMagicCharging = true;

            // Set the flag to true when casting starts
            gameObject.GetComponent<PlayerMovement>().isMagicButtonHeld = true;

            CastMagic();

            magicCoroutine = StartCoroutine(CastMagicWhileHeld());
        }
    }

    private void OnMagicCanceled(InputAction.CallbackContext context)
    {
        if (isMagicCharging && magicCoroutine != null)
        {
            StopCoroutine(magicCoroutine);
        }
        isMagicCharging = false;

        // Set the flag to false when casting stops
        gameObject.GetComponent<PlayerMovement>().isMagicButtonHeld = false;
    }


    private void CastMagic()
    {
        if (magicDirection != Vector2.zero && !isMagicOnCooldown)
        {
            GameObject magicProjectile = Instantiate(magic, transform.position, transform.rotation);
            magicProjectile.SetActive(true);
            Projectile_Test projectileScript = magicProjectile.GetComponent<Projectile_Test>();
            if (projectileScript != null)
            {
                projectileScript.GetDirection(magicDirection); // keeps the initial direction
            }
            Debug.Log("Magic casted");

            // starts cooldown
            StartCoroutine(MagicCooldown());
        }
    }

    private IEnumerator CastMagicWhileHeld()
    {
        while (isMagicCharging)
        {
            if (!isMagicOnCooldown)
            {
                CastMagic(); 
            }

            yield return null;
        }
    }

    private IEnumerator MagicCooldown()
    {
        isMagicOnCooldown = true;
        yield return new WaitForSeconds(magicCooldownTime); // waits for cooldown
        isMagicOnCooldown = false;
    }

    private void OnMeleePerformed(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
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
        {
            gameObject.SetActive(false);
        }
            
        
    }
}
