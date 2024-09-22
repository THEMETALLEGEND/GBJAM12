using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class Roamer_Script : MonoBehaviour, Enemy
{
    private bool canMove = true;
    private int health = 2;
    [HideInInspector] public int room { get; set; }
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool choosed_direction;
    private bool isInKnockBack;
    private Room_TransitionCollision roomsTransition;
    public GameObject coin_prefab;
    private Animator anim;
    private CircleCollider2D circleCollider;

    #region Editor Settings

    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float flashDuration = 0.5f;

    [Tooltip("Audio clip for damage sound.")]
    public AudioClip enemyDamageClip;

    #endregion

    private Material originalMaterial;
    private Coroutine flashRoutine;
    private GBSoundController soundController;

    void Start()
    {
        soundController = FindObjectOfType<GBSoundController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalMaterial = GetComponent<SpriteRenderer>().material;
        circleCollider = GetComponent<CircleCollider2D>();
        StartCoroutine(MovementRoutine());

        roomsTransition = GameObject.Find("Player").transform.GetChild(1).gameObject.GetComponent<Room_TransitionCollision>();
    }

    public IEnumerator DropCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(0, 100) < 50)
            {
                GameObject Coin = Instantiate(coin_prefab, transform.position, transform.rotation);
            }
        }
        canMove = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isInKnockBack)
        {
            return;
        }

        if (canMove)
        {
            rb.velocity = moveDirection;
            UpdateAnimation();
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.Play("roamer_idle");
        }

        // Check for overlaps with player using OverlapCircle
        CheckPlayerOverlap();
    }

    public void Damage(int damageAmount)
    {
        soundController.PlaySound(enemyDamageClip);
        if (roomsTransition.actual_Room == room)
        {
            health -= damageAmount;
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashRoutine());
            if (health <= 0)
            {
                isInKnockBack = true;
                StartCoroutine(DropCoins(1));
            }
        }
    }

    public void SetRoom(int r)
    {
        room = r;
    }

    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            if (!isInKnockBack)
            {
                moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                canMove = true;
                yield return new WaitForSeconds(2f);

                canMove = false;
                rb.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return null;
            }
        }
    }

    // Check for overlaps with player using OverlapCircle
    // Check for overlaps with player using OverlapCircle
    private void CheckPlayerOverlap()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (Collider2D hit in hits)
        {
            // Comparando o nome do objeto ao invés da tag
            if (hit.name == "Player" && health > 0)
            {
                hit.GetComponent<PlayerAttacking>().TakeDamage(1, transform.position);
            }
        }
    }


    public void KnockBack_(Vector2 knockbackDirection)
    {
        Vector2 direction = (transform.position - (Vector3)knockbackDirection).normalized;
        StartCoroutine(KnockBackRoutine(direction));
    }

    private IEnumerator KnockBackRoutine(Vector2 knockbackDirection)
    {
        isInKnockBack = true;
        canMove = false;
        rb.velocity = knockbackDirection * 3f;

        yield return new WaitForSeconds(1f);

        canMove = true;
        rb.velocity = Vector2.zero;
        isInKnockBack = false;
    }

    private void UpdateAnimation()
    {
        if (moveDirection.x < 0)
        {
            anim.Play("roamer_moving_left");
        }
        else if (moveDirection.x > 0)
        {
            anim.Play("roamer_moving_right");
        }
        else
        {
            anim.Play("roamer_idle");
        }
    }

    private IEnumerator FlashRoutine()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
    }

    // Draw the overlap circle in the scene view for easier debugging
    private void OnDrawGizmos()
    {
        if (circleCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
