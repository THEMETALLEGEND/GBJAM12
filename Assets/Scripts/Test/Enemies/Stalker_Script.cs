using Pathfinding;
using System.Collections;
using UnityEngine;
using GBTemplate;

public class Stalker_Script : MonoBehaviour, Enemy
{
	public AudioClip EnemyDamage;
	public Transform target;
	private Rigidbody2D rb;
	private bool isPaused = false;
	private int health = 2;
	public int room { get; set; }
	private Room_TransitionCollision roomsTransition;

	public GameObject coin_prefab;
	private Animator anim;
	private bool isAttacking = false;
	private Collider2D attackRangeCollider;
	private float attackCooldown = 2f;

	private GBSoundController soundController;

	#region Editor Settings

	[Tooltip("Material to switch to during the flash.")]
	[SerializeField] private Material flashMaterial;

	[Tooltip("Duration of the flash.")]
	[SerializeField] private float duration = 0.5f;

	#endregion
	#region Private Fields

	private SpriteRenderer spriteRenderer;
	private Material originalMaterial;
	private Coroutine flashRoutine;
	#endregion

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalMaterial = spriteRenderer.material;

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		target = FindObjectOfType<PlayerMovement>().transform;
		attackRangeCollider = GetComponent<Collider2D>();
		soundController = FindObjectOfType<GBSoundController>();

		roomsTransition = GameObject.Find("Player").transform.GetChild(1).gameObject.GetComponent<Room_TransitionCollision>();

		FindRoomTransition();
		StartCoroutine(transitionTime());
	}

	private void FindRoomTransition()
	{
		Room_TransitionCollision transitionCollider = GetComponentInParent<Room_TransitionCollision>();
		if (transitionCollider != null)
		{
			roomsTransition = transitionCollider;
		}
	}

	private IEnumerator transitionTime()
	{
		AIPath ai = GetComponent<AIPath>();
		ai.canMove = false;
		yield return new WaitForSeconds(1f);
		ai.canMove = true;
	}

	private void Update()
	{
		if (target != null && !isAttacking)
		{
			Vector2 direction = target.position - transform.position;
			anim.SetFloat("MoveX", direction.x);
			CheckAttackRange();
		}
	}

	private void CheckAttackRange()
	{
		Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRangeCollider.bounds.extents.x, LayerMask.GetMask("Player"));

		if (hit != null && !isAttacking)
		{
			StartCoroutine(PerformAttack());
		}
	}

	private IEnumerator PerformAttack()
	{
		isAttacking = true;
		anim.SetBool("IsAttacking", true);

		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

		Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRangeCollider.bounds.extents.x, LayerMask.GetMask("Player"));
		if (hit != null)
		{
			target.GetComponent<PlayerAttacking>().TakeDamage(1, transform.position);
		}

		anim.SetBool("IsAttacking", false);
		yield return new WaitForSeconds(attackCooldown);
		isAttacking = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRangeCollider.bounds.extents.x);
	}

	public void SetRoom(int r)
	{
		room = r;
	}

	public void Damage(int damageAmount)
	{
		if (soundController != null)
		{
			soundController.PlaySound(EnemyDamage); // Substitua "HitSound" pelo nome correto do seu som
		}

		if (roomsTransition.actual_Room == room)
		{
			health -= damageAmount;
			StartCoroutine(FlashRoutine());
			if (health <= 0)
			{
				StartCoroutine(DropCoins(1));
			}
		}
	}
	private IEnumerator FlashRoutine()
	{
		// Swap to the flashMaterial.
		spriteRenderer.material = flashMaterial;

		// Pause the execution of this function for "duration" seconds.
		yield return new WaitForSeconds(duration);

		// After the pause, swap back to the original material.
		spriteRenderer.material = originalMaterial;

		// Set the routine to null, signaling that it's finished.
		flashRoutine = null;
	}

	public IEnumerator DropCoins(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject Coin = Instantiate(coin_prefab, transform.position, transform.rotation);
		}
		AIPath ai = GetComponent<AIPath>();
		ai.canMove = false;
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	public void KnockBack_(Vector2 knockbackDirection)
	{
		Vector2 direction = (transform.position - (Vector3)knockbackDirection).normalized;
		StartCoroutine(KnockBackRoutine(direction));
	}

	private IEnumerator KnockBackRoutine(Vector2 knockbackDirection)
	{
		AIPath ai = GetComponent<AIPath>();
		ai.canMove = false;
		rb.velocity = knockbackDirection * 2f;
		yield return new WaitForSeconds(1f);
		rb.velocity = Vector2.zero;
		ai.canMove = true;
	}
}
