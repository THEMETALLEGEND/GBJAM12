using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraFollower : MonoBehaviour
{
    public Camera mainCamera;  
    public Camera canvasCamera; 

    void Start()
    {
        SyncCameras();
    }

    void LateUpdate()
    {
        SyncCameras();
    }

    void SyncCameras()
    {
        canvasCamera.transform.position = mainCamera.transform.position;
        canvasCamera.transform.rotation = mainCamera.transform.rotation;
        canvasCamera.orthographicSize = mainCamera.orthographicSize;
        canvasCamera.orthographic = mainCamera.orthographic;
        canvasCamera.fieldOfView = mainCamera.fieldOfView; 
    }
}
