using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CanvasCameraFollower : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;  
    public CinemachineVirtualCamera canvasCamera; 

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
        Vector3 mainCamPosition = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.transform.position;
        Quaternion mainCamRotation = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.transform.rotation;

        canvasCamera.transform.position = mainCamPosition;
        canvasCamera.transform.rotation = mainCamRotation;

    }

}
