using System.Collections.Generic;
using UnityEngine;

public class Map_Controller : MonoBehaviour
{
    public LayerMask doorLayer; // Defina o LayerMask para identificar salas e portas
    public float detectionRadius = 1f; // Raio para detectar portas

    public void OnPlayerEnterRoom(Room currentRoom)
    {
        // Verifica salas adjacentes e portas
        CheckAdjacentRooms(currentRoom);
    }

    private void CheckAdjacentRooms(Room currentRoom)
    {
        // Usando BoxCollider2D para detectar portas adjacentes
        Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(currentRoom.transform.position, new Vector2(detectionRadius, detectionRadius), 0f, doorLayer);

        foreach (Collider2D collider in nearbyColliders)
        {
            Room adjacentRoom = collider.GetComponent<Room>();
            if (adjacentRoom != null && !adjacentRoom.IsVisited)
            {
                // Verifica a posi��o da porta para determinar a dire��o
                Vector2 direction = GetDirectionFromCollider(collider);
                adjacentRoom.UpdateRoomIcons(direction); // Atualiza os �cones com base na dire��o
                adjacentRoom.SetAccessible(); // Define salas adjacentes como acess�veis (cor amarela)
            }
        }
    }

    private Vector2 GetDirectionFromCollider(Collider2D collider)
    {
        // Calcula a dire��o da porta em rela��o � sala atual
        Vector2 direction = Vector2.zero;
        // Aqui voc� pode usar a posi��o do collider e a posi��o da sala atual para determinar a dire��o
        // Exemplo fict�cio para fins ilustrativos:
        Vector2 offset = collider.transform.position - transform.position;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            direction = offset.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            direction = offset.y > 0 ? Vector2.up : Vector2.down;
        }
        return direction;
    }
}
