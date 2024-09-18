using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamChange : MonoBehaviour
{
    private float teleportDistance = 2f;
    private CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera canvasCamera;
    private BoxCollider2D col;
    [HideInInspector] public bool isTransitioning = false;


    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        cam = transform.parent.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RoomTransition(other));
        }
    }

    private IEnumerator RoomTransition(Collider2D player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        // For subsequent transitions, disable player movement and stop the player immediately
        isTransitioning = true;
        playerMovement.isAllowedToMove = false;
        playerMovement.rb.velocity = Vector2.zero;

        // Delay before switching the camera
        yield return new WaitForSeconds(0.5f);

        // Switch camera
        cam.Priority = 10;
        UpdateMainCamera(cam);

        // Teleport player forward based on the entered side
        TeleportPlayer(player);

        // Delay during the camera transition
        yield return new WaitForSeconds(1.5f);

        // Enable player movement after the transition
        playerMovement.isAllowedToMove = true;
        isTransitioning = false;

    }

    private void TeleportPlayer(Collider2D player)
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 triggerPosition = transform.position;

        // Determine the side from which the player entered the trigger
        Vector2 direction = playerPosition - triggerPosition;

        // Teleport player based on the entered direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))  // Entered from the left or right
        {
            if (direction.x > 0)  // Entered from the right, teleport left (negative X)
            {
                player.transform.position = new Vector2(playerPosition.x - teleportDistance, playerPosition.y);
            }
            else  // Entered from the left, teleport right (positive X)
            {
                player.transform.position = new Vector2(playerPosition.x + teleportDistance, playerPosition.y);
            }
        }
        else  // Entered from the top or bottom
        {
            if (direction.y > 0)  // Entered from the top, teleport down (negative Y)
            {
                player.transform.position = new Vector2(playerPosition.x, playerPosition.y - teleportDistance);
            }
            else  // Entered from the bottom, teleport up (positive Y)
            {
                player.transform.position = new Vector2(playerPosition.x, playerPosition.y + teleportDistance);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.Priority = 0;
        }
    }

    // I wasn't able to make the canvas work with the overlay by how the template worked, so I made a camera to render the canvas and follow the main camera
    void UpdateMainCamera(CinemachineVirtualCamera cameratocanvas)
    {
        // Sends to the canvas script which cam it has to follow
        CanvasCameraFollower canvasCameraFollower = canvasCamera.GetComponent<CanvasCameraFollower>();
        canvasCameraFollower.mainCamera = cameratocanvas;
    }
}
