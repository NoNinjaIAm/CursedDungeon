using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using static GameEnums;

public class SpawnManager : MonoBehaviour
{
    public struct SpawnSettings
    {
        public int minDisSpawnAmount;
        public int maxDisSpawnAmount;
        public float lockMovesProbability;
    }
    private Dictionary<GameDifficulty, SpawnSettings> spawnSettings;
    private GameDifficulty currentDifficulty;

    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private GameObject[] distractionPrefabs;
    [SerializeField] private float spawnXBound = 7.5f;
    [SerializeField] private float spawnYBound = 4.5f;

    [SerializeField] private Vector2 noSpawnZoneMin = new Vector2(-1.5f, -4.5f);
    [SerializeField] private Vector2 noSpawnZoneMax = new Vector2(1.5f, 1.75f);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnChallengeStart += OnChallengeStart;
        GameManager.OnDifficultyChanged += OnDifficultyChanged;
    }

    private void Awake()
    {
        spawnSettings = new Dictionary<GameDifficulty, SpawnSettings>
        {
            { GameDifficulty.Easy, new SpawnSettings {minDisSpawnAmount = 5, maxDisSpawnAmount = 10, lockMovesProbability = 0.1f} },
            { GameDifficulty.Medium, new SpawnSettings {minDisSpawnAmount = 15, maxDisSpawnAmount = 40, lockMovesProbability = 0.3f} },
            { GameDifficulty.Hard, new SpawnSettings {minDisSpawnAmount = 40, maxDisSpawnAmount = 75, lockMovesProbability = 0.6f} },
            { GameDifficulty.VeryHard, new SpawnSettings {minDisSpawnAmount = 50, maxDisSpawnAmount = 100, lockMovesProbability = 0.9f} }
        };

        // Easy by default
        currentDifficulty = GameDifficulty.Easy;
    }

    private void OnChallengeStart()
    {
        SpawnLock();
        SpawnDistractions();
    }

    private void SpawnLock()
    {
        SpawnSettings settings = spawnSettings[currentDifficulty]; // get current difficulty
        Vector2 spawnPos = GetRandomSpawnPos();
        MovementAI movementAI;

        var lockInstance = Instantiate(lockPrefab, spawnPos, lockPrefab.transform.rotation);
        
        // If the lock moves give it an AI, else give it a null AI
        if(settings.lockMovesProbability > Random.Range(0.0f, 1.0f))
        {
            movementAI = MovementAI.PingPong;
            Debug.Log("Lock will move. Move Prob: " + settings.lockMovesProbability);
        }
        else movementAI = MovementAI.None;
        
        
        
        // Set the AI
        EntityMovement movement = lockInstance.GetComponent<EntityMovement>();
        if (movement != null)
        {
            movement.MovementAI = movementAI;
            movement.LockRotation = true;
        }
        else
        {
            Debug.LogWarning("WARNING: SpawnManager tried to grab a null EntityMovement Component!!!");
        }
    }

    private void OnDifficultyChanged(GameDifficulty difficulty)
    {
        switch(difficulty)
        {
            case GameDifficulty.Easy:
                currentDifficulty = GameDifficulty.Easy;
                break;
            case GameDifficulty.Medium:
                currentDifficulty = GameDifficulty.Medium;
                break;
            case GameDifficulty.Hard:
                currentDifficulty = GameDifficulty.Hard;
                break;
            case GameDifficulty.VeryHard:
                currentDifficulty = GameDifficulty.VeryHard;
                break;
        }
    }

    private void SpawnDistractions()
    {
        SpawnSettings settings = spawnSettings[currentDifficulty]; // get current difficulty
        int amount = Random.Range(settings.minDisSpawnAmount, settings.maxDisSpawnAmount);

        Debug.Log("Spawning " + amount + " distractions");

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
        return values[Random.Range(0, values.Length-1)];
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
        GameManager.OnDifficultyChanged -= OnDifficultyChanged;
    }
}
