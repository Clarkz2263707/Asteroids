/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 500f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireCooldown = .5f;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float invincibilityTime = 3f;
    [SerializeField] private Vector3 respawnPosition = default;
    [SerializeField] private float InvincinbilityBlink = 8f;

    private int currentLives;
    private float rotationInput;
    private float thrustInput;
    private float firerateCooldown = -Mathf.Infinity;
    private Collider2D playerCollider;
    private SpriteRenderer[] spriteRenderers;
    private bool isInvincible;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        currentLives = startingLives;
        if (respawnPosition == default) respawnPosition = Vector3.zero;
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");
        thrustInput = Input.GetAxis("Vertical");
        HandleRotation();
        HandleFire();
        HandleHyperspace();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    private void HandleRotation()
    {
        if (Mathf.Approximately(rotationInput, 0f) == false)
        {
            float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationAmount);
        }
    }

    private void HandleThrust()
    {
        if (rb == null)
            return;

        if (thrustInput > 0f)
        {
            rb.AddForce(transform.up * thrustInput * thrustForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= firerateCooldown + fireCooldown)
            {
                FireBullet();
                firerateCooldown = Time.time;
            }
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            TeleportToRandomLocation();
        }
    }

    private void TeleportToRandomLocation()
    {
        float randomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomY = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);
        transform.position = new Vector3(randomX, randomY, transform.position.z);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
