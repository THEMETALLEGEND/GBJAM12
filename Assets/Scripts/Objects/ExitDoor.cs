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
			StartCoroutine(ChangeScene());
		}
	}
    public IEnumerator ChangeScene()
    {
        disp = FindObjectOfType<GBConsoleController>();
        canvasTMPs.enabled = false;
        yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		if(currentSceneIndex == 5 || currentSceneIndex == 4)
		{
            PlayerPrefs.SetFloat("rangeUpgrade", 0); // THE UPGRADES RESET HERE
            PlayerPrefs.SetFloat("magicCooldown", 0);
        }
        SceneManager.LoadScene(currentSceneIndex+1);
    }
}
