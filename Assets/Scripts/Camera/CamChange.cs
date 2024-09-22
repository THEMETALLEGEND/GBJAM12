using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamChange : MonoBehaviour
{
    private float teleportHorizontalDistance = 2.2f;
    private float teleportVerticalDistance = 2.7f;
    private CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera canvasCamera;
    private BoxCollider2D col;
    [HideInInspector] public bool isTransitioning = false;
    [SerializeField] private bool isSpawnRoom = false;

    public Canvas canvas;
    public Canvas TMP_canvas;
    public AudioClip closeSound;
    private AudioSource source;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        cam = transform.parent.GetComponent<CinemachineVirtualCamera>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSpawnRoom)
        {
            StartCoroutine(RoomTransition(other));
        }
        else if (other.CompareTag("Player") && isSpawnRoom)
            isSpawnRoom = false;
    }

    private IEnumerator RoomTransition(Collider2D player)
    {
        Collider2D playerParent = player.transform.parent.GetComponent<Collider2D>();
        PlayerMovement playerMovement = playerParent.GetComponent<PlayerMovement>();

        foreach (Transform child in canvas.transform) // disables all canvas childs
        {
            child.gameObject.SetActive(false);
        }

        TMP_canvas.enabled = false;

        isTransitioning = true;
        playerMovement.isAllowedToMove = false;
        playerMovement.rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        cam.Priority = 10;
        UpdateMainCamera(cam);

        TeleportPlayer(playerParent);

        yield return new WaitForSeconds(1.5f);

        foreach (Transform child in canvas.transform) // enables all canvas childs
        {
            child.gameObject.SetActive(true);
        }
        TMP_canvas.enabled = true;

        playerMovement.isAllowedToMove = true;
        isTransitioning = false;
        yield return new WaitForSeconds(1f);
        
    }

    public CinemachineVirtualCamera ReturnCamera()
    {
        return cam;
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
                player.transform.position = new Vector2(playerPosition.x - teleportHorizontalDistance, playerPosition.y);
            }
            else  // Entered from the left, teleport right (positive X)
            {
                player.transform.position = new Vector2(playerPosition.x + teleportHorizontalDistance, playerPosition.y);
            }
        }
        else  // Entered from the top or bottom
        {
            if (direction.y > 0)  // Entered from the top, teleport down (negative Y)
            {
                player.transform.position = new Vector2(playerPosition.x, playerPosition.y - teleportVerticalDistance);
            }
            else  // Entered from the bottom, teleport up (positive Y)
            {
                player.transform.position = new Vector2(playerPosition.x, playerPosition.y + teleportVerticalDistance);
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
