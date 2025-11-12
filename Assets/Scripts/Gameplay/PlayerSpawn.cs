using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine; // Required for Vector3

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false;
            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);
            player.health.Increment();
            
            // --- MODIFICATION START ---
            // Check if the player has a stored respawn position (set by Checkpoint.cs).
            // We assume model.player (which is PlayerController) has a public respawnPosition field.
            if (player.respawnPosition != Vector3.zero)
            {
                player.Teleport(player.respawnPosition);
            }
            else
            {
                // Fallback to the original spawn point if no checkpoint has been hit.
                player.Teleport(model.spawnPoint.transform.position);
            }
            // --- MODIFICATION END ---
            
            player.jumpState = PlayerController.JumpState.Grounded;
            player.animator.SetBool("dead", false);
            model.virtualCamera.Follow = player.transform;
            model.virtualCamera.LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}