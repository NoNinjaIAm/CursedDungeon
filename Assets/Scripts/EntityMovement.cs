using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class EntityMovement : MonoBehaviour
{
    public float speed = 5f;  // Movement speed
    private Rigidbody2D rb;   // Rigidbody2D for physics handling
    private Vector2 moveDirection;  // Direction of movement
    private float spinSpeed;  // Direction of movement

    public GameEnums.MovementAI MovementAI { private get; set; } = GameEnums.MovementAI.Bounce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDirection = transform.up;  // Start moving in the direction of 'up' on the entity
        // Events
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStoppedOutcome;
        spinSpeed = Random.Range(-360f, 360f);

    }

    private void FixedUpdate()
    {
        switch (MovementAI)
        {
            case GameEnums.MovementAI.Bounce:
                BounceAI();
                break;
            case GameEnums.MovementAI.Float:
                FloatAI();
                break;
        }
    }

    private void BounceAI()
    {  
        // Always move in the current direction (forward)
        rb.linearVelocity = moveDirection * speed;

        // Ensure the entity's rotation matches the direction of movement
        if (moveDirection != Vector2.zero)
        {
            transform.up = moveDirection;  // Keep the top of the entity facing the movement direction
        }    
    }

    private void FloatAI()
    {
        rb.angularVelocity = spinSpeed;
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
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        
        // Disable myself afterwards
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
    }
}
