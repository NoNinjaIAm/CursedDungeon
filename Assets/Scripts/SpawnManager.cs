using UnityEditor.SceneManagement;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private float spawnXBound = 7.5f;
    [SerializeField] private float spawnYBound = 4.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnLock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnLock()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-spawnXBound, spawnXBound), Random.Range(-spawnYBound, spawnYBound));

        Instantiate(lockPrefab, spawnPos, lockPrefab.transform.rotation);
    }
}
