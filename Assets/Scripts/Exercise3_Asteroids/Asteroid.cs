/*
 * Assignment: Asteroids Game - Asteroid Script - PART 2
 * 
 * Objective: Create a functional asteroid script. This script will be responsible for the functionality of the asteroids.
 * this should include initial velocity, angular velocity, and breaking into smaller asteroids when destroyed.
 * Remember, asteroids should only spawn through the AsteroidSpawner script. 
 
* Requirements:
* 1. The asteroid should start with a constant speed but a random angular velocity. Both of these are set in the Rigidbody2D
*       The movement direction of the asteroid should not change. 
*       Hint: All movement for the asteroid should be done via a Rigidbody2D and should be able to be set at Start.
* 2. When the asteroid is destroyed, it should spawn two smaller asteroids if it is not already the smallest size. 
*       Hint: How can you use a function to set the AsteroidSpawner variable from a different script?
* 3. When the astroid hits the player, it should destroy the player. 
*/

using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private AsteroidSpawner spawner;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            return;

        velocity = (Vector2)transform.up * speed;
        rb.linearVelocity = velocity;
    }

    void Update()
    {
    }

    //basically calls upon the spawner/sets the spawner to actually spawn the asteroids.
    public void SetSpawner(AsteroidSpawner asteroidSpawner)
    {
        spawner = asteroidSpawner;
    }

    //what happens when the asteroid is broken if children are spawned or not
    private void BreakAsteroid()
    {
        if (size == AsteroidSize.Small)
            return;

        AsteroidSize childSize = size == AsteroidSize.Large ? AsteroidSize.Medium : AsteroidSize.Small;

        if (spawner != null)
        {
            SpawnChildren(childSize);
        }
    }

    //sets the spawn location of the children of the asteroid
    private void SpawnChildren(AsteroidSize childSize)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(0.5f, 1.0f);
            Vector3 spawnPos = transform.position + (Vector3)offset;
            spawner.SpawnAsteroid(spawnPos, childSize);
        }
    }

    //Destruction of the asteroid if it is hit by both the player and bullet.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            Destroy(collision.gameObject);
            BreakAsteroid();
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            BreakAsteroid();
            Destroy(gameObject);
        }
    }
}
