using UnityEditor.SceneManagement;
using UnityEngine;

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
        SpawnDistractions(100);
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
            Vector2 spawnPos = GetRandomSpawnPos();
            Quaternion rotationPos = Quaternion.Euler(distractionPrefab.transform.rotation.x, distractionPrefab.transform.rotation.y, GetRandomAxisRotation());
            Instantiate(distractionPrefab, spawnPos, rotationPos);
        }
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
