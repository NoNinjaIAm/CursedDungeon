using UnityEngine;

public class MenuEyeballMovement : MonoBehaviour
{
    private float speed = 10f; // Movement speed
    private float waitTime = 4f; // Time to wait at each destination
    private float maxDistanceFromStart = 60f; // Maximum distance from the starting position
    private Vector2 startPosition;
    private Vector2 currentTarget;
    private bool isWaiting = false;
    private bool lastFullscreenState;

    void Start()
    {
        startPosition = transform.position;  // Save the starting position
        MoveToRandomPosition();  // Move to an initial random target
        lastFullscreenState = Screen.fullScreen;
    }

    void Update()
    {
        if (!isWaiting)
        {
            MoveTowardsTarget();
        }
        CheckChangeInScreenState();
    }

    private void MoveTowardsTarget()
    {
        // Move the object towards the target position
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // Once we reach the target, wait for a few seconds and then pick a new target
        if (AreFloatsClose(transform.position.x, currentTarget.x) && AreFloatsClose(transform.position.y, currentTarget.y))
        {
            isWaiting = true;
            Invoke("MoveToRandomPosition", waitTime); // Wait for the specified time before moving
        }
    }

    bool AreFloatsClose(float a, float b, float tolerance = 0.5f)
    {
        return Mathf.Abs(a - b) <= tolerance;
    }

    private void CheckChangeInScreenState()
    {
        if (Screen.fullScreen != lastFullscreenState)
        {
            Debug.Log("Fullscreen state changed! Killing eyeball");

            // Kill movement if resolution has changed
            Destroy(this);
        }
    }

    private void MoveToRandomPosition()
    {
        // Pick a random position within the bounds
        float randomX = Random.Range(startPosition.x - maxDistanceFromStart, startPosition.x + maxDistanceFromStart);
        float randomY = Random.Range(startPosition.y - maxDistanceFromStart, startPosition.y + maxDistanceFromStart);
        currentTarget = new Vector2(randomX, randomY);

        isWaiting = false; // Start moving again
    }
}
