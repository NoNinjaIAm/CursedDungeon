using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 5f;  // Movement speed
    private Rigidbody2D rb;   // Rigidbody2D for physics handling
    private Vector2 moveDirection;  // Direction of movement
    private bool isChallengeActive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = transform.up;  // Start moving in the direction of 'up' on the entity
        
        // Events
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStoppedOutcome;
    }

    private void Update()
    {
        if (isChallengeActive) // Challenge is active
        {
            // Always move in the current direction (forward)
            rb.linearVelocity = moveDirection * speed;

            // Ensure the entity's rotation matches the direction of movement
            if (moveDirection != Vector2.zero)
            {
                transform.up = moveDirection;  // Keep the top of the entity facing the movement direction
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the normal of the surface the entity hit (the wall's surface normal)
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Reflect the current movement direction off the wall's surface
        moveDirection = Vector2.Reflect(moveDirection, collisionNormal).normalized;
    }

    private void OnPlayerStoppedOutcome(bool outcome)
    {
        isChallengeActive = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
    }
}
