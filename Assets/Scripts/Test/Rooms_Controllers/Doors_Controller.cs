using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;
using Cinemachine;

public class Doors_Controller : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask doorLayer;
    private Collider2D roomCollider;
    public AudioClip openSound;
    public AudioClip closeSound;
    private GBSoundController soundController;
    public bool isAlreadyOpen = false;
    private bool isOnBattle = false;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineImpulseSource impulseSource; 

    private void Start()
    {
        roomCollider = GetComponent<Collider2D>();
        soundController = FindObjectOfType<GBSoundController>();
        impulseSource = GetComponent<CinemachineImpulseSource>(); 
    }

    void Update()
    {
        if (isOnBattle)
        {
            Collider2D[] EnemiesinRoom = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, LayerMask.GetMask("Enemy"));
            if (EnemiesinRoom.Length <= 0)
            {
                StartCoroutine(HandleDoors(true));
            }
        }
    }

    public void OnRoomEnter()
    {
        Collider2D[] EnemiesinRoom = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, LayerMask.GetMask("EnemySpawner"));
        if (EnemiesinRoom.Length < 1)
        {
            Debug.Log("OpeningDoors");
            StartCoroutine(HandleDoors(true));
        }
        else
        {
            Debug.Log("ClosingDoors");
            StartCoroutine(HandleDoors(false));
            gameObject.GetComponent<Room>().FindAndActivateSpawners();
        }
    }

    private IEnumerator HandleDoors(bool open)
    {
        Collider2D[] doors = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f, doorLayer);
        yield return new WaitForSeconds(2f);
        foreach (var door in doors)
        {
            Debug.Log($"Detected door: {door.name}");
            Transform openDoor = door.transform.GetComponentInChildren<Transform>().Find("OpenDoor");
            Transform closedDoor = door.transform.GetComponentInChildren<Transform>().Find("ClosedDoor");

            if (openDoor != null && closedDoor != null)
            {
                if (open)
                {
                    openDoor.gameObject.SetActive(true);
                    closedDoor.gameObject.SetActive(false);
                    if (!isAlreadyOpen)
                    {
                        soundController.PlaySound(openSound);
                        isAlreadyOpen = true;
                    }
                }
                else
                {
                    openDoor.gameObject.SetActive(false);
                    closedDoor.gameObject.SetActive(true);

                    if (impulseSource != null) 
                    {
                        impulseSource.GenerateImpulse();
                    }

                    soundController.PlaySound(closeSound);
                    isOnBattle = true;
                
                }
            }
        }
    }
}
