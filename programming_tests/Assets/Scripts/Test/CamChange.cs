using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamChange : MonoBehaviour
{
	public CinemachineVirtualCamera cam;
	public CinemachineVirtualCamera previousCam;
	private BoxCollider2D col;

	private void Awake()
	{
		col = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			// Get the CinemachineBrain from the main camera
			CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

			// Get the current active virtual camera
			previousCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;

			previousCam.Priority = 0;

			cam.Priority = 10;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			cam.Priority = 0;
			previousCam.Priority = 10;
		}
	}
}
