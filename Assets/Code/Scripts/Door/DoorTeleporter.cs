using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTeleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private bool playerInside = false;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    private void Update()
    {
        if (!playerInside || player == null)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        player.position = destination.position;
    }
}