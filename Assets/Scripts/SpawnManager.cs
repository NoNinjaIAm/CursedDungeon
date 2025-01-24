using UnityEditor.SceneManagement;
using UnityEngine;
using static GameEnums;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private GameObject distractionPrefab;
    [SerializeField] private float spawnXBound = 7.5f;
    [SerializeField] private float spawnYBound = 4.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnChallengeStart += OnChallengeStart;
        
    }
    
    private void OnChallengeStart()
    {
        SpawnLock();
        SpawnDistractions(10);
    }

    private void SpawnLock()
    {
        Vector2 spawnPos = GetRandomSpawnPos();

        Instantiate(lockPrefab, spawnPos, lockPrefab.transform.rotation);
    }

    private void SpawnDistractions(int amount)
    {
        for (int i=0; i<amount; i++)
        {
            // Get values for distraction
            MovementAI movementAI = GetRandomMovementAI();
            Vector2 spawnPos = GetRandomSpawnPos();
            Quaternion rotationPos = Quaternion.Euler(0, 0, GetRandomAxisRotation()) * distractionPrefab.transform.rotation;

            // Instantiate and access the object
            GameObject distraction = Instantiate(distractionPrefab, spawnPos, rotationPos);

            // Set the AI
            EntityMovement movement = distraction.GetComponent<EntityMovement>();
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
        return new Vector2(Random.Range(-spawnXBound, spawnXBound), Random.Range(-spawnYBound, spawnYBound));
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
