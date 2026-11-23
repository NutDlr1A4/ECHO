using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTeleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    [Header("Door Sound")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorSound;
    [SerializeField, Range(0f, 1f)] private float doorVolume = 0.7f;
    [SerializeField] private GameObject openText;

    private bool playerInside = false;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;

            openText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;

            openText.SetActive(false);
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
        if (doorAudioSource != null && doorSound != null)
        {
            doorAudioSource.PlayOneShot(doorSound, doorVolume);
        }

        player.position = destination.position;
    }
}