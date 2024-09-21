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
    [HideInInspector] public Vector2 magicDirection; // direction of the projectile
    private bool isMagicCharging = false;
    private bool isMagicOnCooldown = false;
    public int hp = 6;
    public LayerMask EnemyLayer;

    public float magicCooldownTime = 0.3f; // cooldown between magics
    public float invincibilityTime = 1.5f; // time during which the player is invincible after taking damage

    private Coroutine magicCoroutine;
    private bool isInvincible = false; // variable to track invincibility
    private SpriteRenderer spriteRenderer; // reference to sprite renderer
    //private Color originalColor; // store the original color of the player

    public UI_Controller ui;

    public float range; // the range of the magic shot.

    public AudioClip damageTaken;
    public AudioClip magicShot;
    private AudioSource sourceAudio;

    void Start()
    {
        sourceAudio = GetComponent<AudioSource>();
        meleeCollider.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>(); // get the sprite renderer for the player
        //originalColor = spriteRenderer.color; // store the original color at the start
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
        sourceAudio.clip = magicShot;
        sourceAudio.Play();
        if (magicDirection != Vector2.zero && !isMagicOnCooldown)
        {
            GameObject magicProjectile = Instantiate(magic, transform.position, transform.rotation);
            magicProjectile.SetActive(true); // sets the instantiated projectile as active since it starts disabled to prevent bugs
            magicProjectile.GetComponent<Projectile_Test>().SetRange(range);
            Projectile_Test projectileScript = magicProjectile.GetComponent<Projectile_Test>();
            if (projectileScript != null)
            {
                projectileScript.GetDirection(magicDirection); // sends the direction the player was facing when started pressing to the projectile code.
            }
            Debug.Log("Magic casted");

            StartCoroutine(MagicCooldown()); // starts cooldown
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
            Transform collider = transform.Find("melee_collider");
            collider.gameObject.SetActive(true); //enables the melee attack collider
            collider.GetComponent<Melee_CollisionScript>().is_Attacking = true;
            if (collider != null)
                StartCoroutine(AttackCooldown(collider.gameObject));
        }
    }

    IEnumerator AttackCooldown(GameObject melee)
    {
        PlayerMovement mov = GetComponent<PlayerMovement>();
        mov.isAllowedToMove = false; // disables movement while the melee attack animation should be on.
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        melee.GetComponent<Melee_CollisionScript>().is_Attacking = false;
        melee.gameObject.SetActive(false);
        mov.isAllowedToMove = true;
    }

    // Method to handle player taking damage
    public void TakeDamage(int dmg, Vector2 enemyPos)
    {
        sourceAudio.clip = damageTaken; //plays the damage taken audio
        sourceAudio.Play(); 
        // If player is invincible, they can't take damage
        if (isInvincible) return;

        Debug.Log("Player took damage");
        hp -= dmg;
        ui.UpdateHeartStates(hp);
        if (hp <= 0)
        {
            gameObject.SetActive(false); // If health is 0, disable the player (or handle death logic here)
        }
        else
        {
            // If still alive, apply knockback and start invincibility
            Vector2 direction = (transform.position - (Vector3)enemyPos).normalized; // calculates the direction of the knockback based on enemy position
            gameObject.GetComponent<PlayerMovement>().ApplyKnockback(direction);
            StartCoroutine(InvincibilityCoroutine()); // Start the invincibility coroutine
        }
    }
    // Coroutine for invincibility

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true; // Set the player to invincible

        // change visibility to make the "flashing" effect of Iframes
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false; 
            yield return new WaitForSeconds(0.25f); 
            spriteRenderer.enabled = true; 
            yield return new WaitForSeconds(0.25f); 
        }
        yield return new WaitForSeconds(0.5f);
        isInvincible = false; 
    }

}
