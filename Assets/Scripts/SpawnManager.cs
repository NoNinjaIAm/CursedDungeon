using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using static GameEnums;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private GameObject[] distractionPrefabs;
    [SerializeField] private float spawnXBound = 7.5f;
    [SerializeField] private float spawnYBound = 4.5f;

    [SerializeField] private Vector2 noSpawnZoneMin = new Vector2(-1.5f, -4.5f);
    [SerializeField] private Vector2 noSpawnZoneMax = new Vector2(1.5f, 1.75f);

    private int distractionsToSpawn = 20; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnChallengeStart += OnChallengeStart;
        
    }
    
    private void OnChallengeStart()
    {
        SpawnLock();
        SpawnDistractions(distractionsToSpawn);
    }

    private void SpawnLock()
    {
        Vector2 spawnPos = GetRandomSpawnPos();

        var lockInstance = Instantiate(lockPrefab, spawnPos, lockPrefab.transform.rotation);
        // Set the AI
        EntityMovement movement = lockInstance.GetComponent<EntityMovement>();
        if (movement != null)
        {
            movement.MovementAI = MovementAI.PingPong;
            movement.LockRotation = true;
        }
        else
        {
            Debug.LogWarning("WARNING: SpawnManager tried to grab a null EntityMovement Component!!!");
        }
    }

    private void SpawnDistractions(int amount)
    {
        for (int i=0; i<amount; i++)
        {
            // Get a random distraction
            var distraction = distractionPrefabs[Random.Range(0, distractionPrefabs.Length)];

            // Get values for distraction
            MovementAI movementAI = GetRandomMovementAI();
            Vector2 spawnPos = GetRandomSpawnPos();
            Quaternion rotationPos = Quaternion.Euler(0, 0, GetRandomAxisRotation()) * distraction.transform.rotation;
            
            // Instantiate and access the object
            var distractionInstance = Instantiate(distraction, spawnPos, rotationPos);

            // Set the AI
            EntityMovement movement = distractionInstance.GetComponent<EntityMovement>();
            if (movement != null)
            {
                movement.MovementAI = movementAI;
            }
            else
            {
                Debug.LogWarning("WARNING: SpawnManager tried to grab a null EntityMovement Component!!!");
            }
        }
    }

    private MovementAI GetRandomMovementAI()
    {
        // Get all enum values as an array
        MovementAI[] values = (MovementAI[])System.Enum.GetValues(typeof(MovementAI));
        // Return the randomly selected value
        return values[Random.Range(0, values.Length)];
    }

    private Vector2 GetRandomSpawnPos()
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

        if(attempts >= maxAttempts) Debug.LogWarning("Warning: SpawnManager could not find valid spawn position in enough attempts! Spawning anyway");
        return spawnPos;

    }

    private bool IsInsideNoSpawnZone(Vector3 pos)
    {
        return pos.x > noSpawnZoneMin.x && pos.x < noSpawnZoneMax.x &&
        pos.y > noSpawnZoneMin.y && pos.y < noSpawnZoneMax.y;
    }

    private float GetRandomAxisRotation ()
    {
        return Random.Range(0f, 360f);
    }

    private void OnDestroy()
    {
        GameManager.OnChallengeStart -= OnChallengeStart;
    }
}
