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
                // Verifica a posição da porta para determinar a direção
                Vector2 direction = GetDirectionFromCollider(collider);
                adjacentRoom.UpdateRoomIcons(direction); // Atualiza os ícones com base na direção
                adjacentRoom.SetAccessible(); // Define salas adjacentes como acessíveis (cor amarela)
            }
        }
    }

    private Vector2 GetDirectionFromCollider(Collider2D collider)
    {
        // Calcula a direção da porta em relação à sala atual
        Vector2 direction = Vector2.zero;
        // Aqui você pode usar a posição do collider e a posição da sala atual para determinar a direção
        // Exemplo fictício para fins ilustrativos:
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
