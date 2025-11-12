using UnityEngine;
using Platformer.Mechanics; // Essential to access the EnemyController class

public class Projectile : MonoBehaviour
{
    // Adjust this to control how long the projectile lasts if it misses.
    public float lifeTime = 5f; 

    void Start()
    {
        // Destroy the projectile after a set time to prevent infinite flight.
        Destroy(gameObject, lifeTime); 
    }
    
    // NOTE: The 'private' modifier is removed here to comply with C# rules 
    // for Unity physics callbacks like OnCollisionEnter2D.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Attempt to get the EnemyController component from the object we hit.
        // This ensures the projectile only damages things that are enemies.
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            // 2. If it is an enemy, call the public function to initiate the death event.
            enemy.TakeDamage(); 
        }

        // 3. Destroy the projectile almost instantly after collision, regardless of what it hit.
        Destroy(gameObject, 1f); 
    }
}