using UnityEngine;
using Platformer.Mechanics; // Essential to access the PlayerController

/// <summary>
/// This script is attached to the Donut (or any checkpoint object).
/// It detects when the player touches it and updates the player's respawn position.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    // We assume the player is the only object with the PlayerController script.
    // We get a reference to the player in Start().
    private PlayerController player;

    void Start()
    {
        // Find the player in the scene when the level starts.
        player = Object.FindFirstObjectByType<PlayerController>();    
    }

    /// <summary>
    /// Called when another collider enters this object's trigger.
    /// NOTE: The Collider2D on the Donut MUST have the 'Is Trigger' box checked.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Attempt to get the PlayerController component from the object that touched the donut.
        PlayerController detectedPlayer = other.GetComponent<PlayerController>();

        if (detectedPlayer != null)
        {
            if (player != null)
            {
                // 1. Update the 'respawnPosition' variable in the PlayerController.
                player.respawnPosition = transform.position;

                // 2. Deactivate the donut so the player can't collect the checkpoint twice.
                // This makes it look like the player 'ate' it.
                gameObject.SetActive(false); 
            }
        }
    }
}