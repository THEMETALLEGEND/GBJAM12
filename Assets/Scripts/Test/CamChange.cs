using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamChange : MonoBehaviour
{
    public Camera cam; 
    public Camera previousCam; 
    public Camera canvasCamera; 
    private BoxCollider2D col;
    public bool returning = false;
    private bool isPlayerInside = false;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPlayerInside)
        {
            isPlayerInside = true;

            if (returning)
            {
                cam.gameObject.SetActive(false);
                previousCam.gameObject.SetActive(true);
                returning = false;

                UpdateMainCamera(previousCam);
            }
            else
            {
                cam.gameObject.SetActive(true);
                previousCam.gameObject.SetActive(false);
                returning = true;

                UpdateMainCamera(cam);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void UpdateMainCamera(Camera cameratocanvas)
    {
        CanvasCameraFollower canvasCameraFollower = canvasCamera.GetComponent<CanvasCameraFollower>();
        canvasCameraFollower.mainCamera = cameratocanvas;
    }
}
