using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class ExitDoor : MonoBehaviour
{
	private PlayerInventory playerInventory;
    public GBConsoleController disp;
	public Canvas canvasTMPs;

    private void Awake()
	{
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && playerInventory.hasKey)
		{
			Debug.Log("Exited the level");
			StartCoroutine(ChangeScene());
		}
	}
    public IEnumerator ChangeScene()
    {
        canvasTMPs.enabled = false;
        yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));
    }
}
