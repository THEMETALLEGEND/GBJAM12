using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class Turret_Enemy : MonoBehaviour, Enemy
{
	public GameObject projectileEnemy;
	public float attackInterval = 2.5f;
	private GameObject transitionObject;
	public AudioClip enemyDamage_;
	[HideInInspector] public int room { get; set; }
	public int health = 2;
	public GameObject coin_prefab;
	private bool isdying;
	private Animator anim;

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
	private GBSoundController soundController;

	void Start()
	{
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalMaterial = spriteRenderer.material;
		StartCoroutine(ShootingProjectile());
		soundController = FindObjectOfType<GBSoundController>();

		transitionObject = GameObject.Find("Player").transform.GetChild(1).gameObject;
	}

	public void Damage(int damageAmount)
	{
		if (transitionObject.GetComponent<Room_TransitionCollision>().actual_Room == room)
		{
			health -= damageAmount;
			if (flashRoutine != null)
			{
				StopCoroutine(flashRoutine);
			}
			flashRoutine = StartCoroutine(FlashRoutine());

			soundController.PlaySound(enemyDamage_);

			if (health <= 0)
			{
                anim.SetBool("Isdead", true);
                StartCoroutine(DropCoins(1));
			}
		}
	}

	public IEnumerator DropCoins(int amount)
	{
		isdying = true;
		for (int i = 0; i < amount; i++)
		{
			GameObject Coin = Instantiate(coin_prefab, transform.position, transform.rotation);
		}
        
        yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	public void SetRoom(int r)
	{
		room = r;
	}

	private IEnumerator FlashRoutine()
	{
		spriteRenderer.material = flashMaterial;
		yield return new WaitForSeconds(duration);
		spriteRenderer.material = originalMaterial;
	}

	IEnumerator ShootingProjectile()
	{
        yield return new WaitForSeconds(1.5f);
        while (!isdying)
		{
			if (transitionObject.GetComponent<Room_TransitionCollision>().actual_Room == room && health > 0)
			{
                Attack();
                yield return new WaitForSeconds(attackInterval);
			}
			else
			{
				yield return null;
			}
		}
    }

	void Attack()
	{
		if (projectileEnemy != null && transitionObject != null && health > 0)
		{
			Vector2 playerPosition = transitionObject.transform.position;
			Vector2 attackDirection = (playerPosition - (Vector2)transform.position).normalized;

			GameObject attack = Instantiate(projectileEnemy, transform.position, transform.rotation);
			attack.SetActive(true);

			Turret_Projectile projectileScript = attack.GetComponent<Turret_Projectile>();
			projectileScript.SetDirection(attackDirection);
		}
	}

	public void KnockBack_(Vector2 knockbackDirection)
	{
		StartCoroutine(KnockBackRoutine(knockbackDirection));
	}

	private IEnumerator KnockBackRoutine(Vector2 knockbackDirection)
	{
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		rb.velocity = knockbackDirection * 1;
		yield return new WaitForSeconds(1);
		rb.velocity = Vector2.zero;
	}
}
