using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using TMPro;

public class EntityMovement : MonoBehaviour
{
    public float speed = 5f;  // Movement speed
    private Rigidbody2D rb;   // Rigidbody2D for physics handling
    [SerializeField] private float spawnXBound = 7.5f;
    [SerializeField] private float spawnYBound = 4.5f;

    [SerializeField] private Vector2 noSpawnZoneMin = new Vector2(-1.5f, -4.5f);
    [SerializeField] private Vector2 noSpawnZoneMax = new Vector2(1.5f, 1.75f);

    // Bounce AI variables
    private Vector2 moveDirection;  // Direction of movement

    // Float AI variables
    private float spinSpeed;  // Direction of movement

    // PingPong AI Variables
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingToTarget;

    // Properties
    public GameEnums.MovementAI MovementAI { private get; set; } = GameEnums.MovementAI.Bounce;
    public bool LockRotation { private get; set; } = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Events
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStoppedOutcome;
        
        switch (MovementAI)
        {
            case GameEnums.MovementAI.Bounce:
                moveDirection = transform.up;  
                break;

            case GameEnums.MovementAI.Float:
                spinSpeed = Random.Range(-360f, 360f);
                break;

            case GameEnums.MovementAI.PingPong:
                startPosition = transform.position;
                targetPosition = GetRandomPos();
                movingToTarget = true;
                break;
        }
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
            case GameEnums.MovementAI.PingPong:
                PingPongAI();
                break;
        }
    }

    private void BounceAI()
    {  
        // Always move in the current direction (forward)
        rb.linearVelocity = moveDirection * speed;

        if(!LockRotation) LookAtDirectionOfTravel();
    }

    // Reflect of wall when hitting entity or wall
    private void BounceAICollision(Collision2D collision)
    {
        // Get the normal of the surface the entity hit (the wall's surface normal)
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Reflect the current movement direction off the wall's surface
        moveDirection = Vector2.Reflect(moveDirection, collisionNormal).normalized;
    }

    private void FloatAI()
    {
        rb.angularVelocity = spinSpeed;
    }

    private void PingPongAI()
    {
        // Determine the current target (either the start position or target position)
        Vector2 currentTarget = movingToTarget ? targetPosition : startPosition;

        // Calculate the movement direction
        moveDirection = (currentTarget - (Vector2)transform.position).normalized;

        // Move the entity toward the current target
        rb.linearVelocity = moveDirection * speed;

        // Check if the entity has reached the target
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            movingToTarget = !movingToTarget; // Toggle between start and target
        }

        if (!LockRotation) LookAtDirectionOfTravel();
    }
    private void PingPongAICollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Assuming walls have the "Wall" tag
        {
            movingToTarget = !movingToTarget;
        }
    }

    private void LookAtDirectionOfTravel()
    {
        // Ensure the entity's rotation matches the direction of movement
        if (moveDirection != Vector2.zero)
        {
            transform.up = moveDirection;  // Keep the top of the entity facing the movement direction
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (MovementAI)
        {
            case GameEnums.MovementAI.Bounce:
                BounceAICollision(collision);
                break;
            case GameEnums.MovementAI.PingPong:
                PingPongAICollision(collision);
                break;
        }
    }

    private void OnPlayerStoppedOutcome(bool outcome)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
    }

    private Vector2 GetRandomPos()
    {
        Vector2 spawnPos;
        int attempts = 0;
        const int maxAttempts = 100;

        // Keep trying to get spawn position that isn't in no spawn zone
        do
        {
            spawnPos = new Vector2(Random.Range(-spawnXBound, spawnXBound), Random.Range(-spawnYBound, spawnYBound));
            attempts++;
        }
        while (IsInsideNoSpawnZone(spawnPos) && attempts < maxAttempts);

        // Spawning anyway despite attempt overwhelm
        return spawnPos;

    }
    private bool IsInsideNoSpawnZone(Vector3 pos)
    {
        return pos.x > noSpawnZoneMin.x && pos.x < noSpawnZoneMax.x &&
        pos.y > noSpawnZoneMin.y && pos.y < noSpawnZoneMax.y;
    }
}
