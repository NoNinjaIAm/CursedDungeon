using UnityEngine;


// // // THE PURPOSE OF THIS SCRIPT IS TO ENSURE THAT A NEEDED MANAGERS ARE GAURENTEED TO BE IN GAME SCENE // // //
// // // WILL DESTROY ITSELF AFTER CHECKING // // //


public class ManagersManager : MonoBehaviour
{
    private void Awake()
    {
        // This script ensures that on every scene load that a GameManager is present
        if (GameManager.Instance == null)
        {
            // Dynamically load the SoundManager prefab
            Instantiate(Resources.Load<GameObject>("GameManager"));
            Debug.Log("GameManager doesn't exist! Spawning it in...");
        }

        // This script ensures that on every scene load that a SoundManager is present
        if (SoundManager.Instance == null)
        {
            // Dynamically load the SoundManager prefab
            Instantiate(Resources.Load<SoundManager>("SoundManager"));
            Debug.Log("SoundManager doesn't exist! Spawning it in...");
        }

        Debug.Log("Destroying Manager Spawner");
        Destroy(gameObject);
    }
}
