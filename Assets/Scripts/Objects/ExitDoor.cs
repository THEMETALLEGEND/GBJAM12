using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
	private PlayerInventory playerInventory;
	private GBConsoleController disp;
	public Canvas canvasTMPs;
	private bool isOpen;
	public bool isExitDoor = false;

	private void Awake()
	{
		playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
	}
	void Update()
	{
		if (playerInventory.hasKey == true && !isOpen)
		{
			GameObject doorop = transform.Find("DoorOpen").gameObject;
			GameObject doorcl = transform.Find("DoorClosed").gameObject;
			doorop.SetActive(true);
			doorcl.SetActive(false);
		}
		//FOR DEBUG ONLY
		if (Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine(ChangeScene());
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && playerInventory.hasKey)
		{
			Debug.Log("Exited the level");
			if (!isExitDoor)
				StartCoroutine(ChangeScene());
			else if (isExitDoor)
				StartCoroutine(ToEndingScene());
		}
	}
	public IEnumerator ChangeScene()
	{
		disp = FindObjectOfType<GBConsoleController>();
		canvasTMPs.enabled = false;
		yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex + 1);
	}
	public IEnumerator ToEndingScene()
	{
		disp = FindObjectOfType<GBConsoleController>();
		canvasTMPs.enabled = false;
		yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2)); // Fade to black over 2 seconds
		SceneManager.LoadScene("EndingScreen"); // Load the ending scene by name
	}
}
