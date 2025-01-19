using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager Instance { get; private set; }
    public static event Action OnChallengeEnd;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Don't destroy on load
        DontDestroyOnLoad(gameObject);

        // Listen for events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerController(); // Gotta find PlayerController on every scene reload
    }

    private void FindPlayerController()
    {
        // Find the PlayerController in the new scene
        playerController = FindFirstObjectByType<PlayerController>();
        
        if (playerController == null) Debug.LogError("ERROR: Player Controller Not Found By GameManager!");
        else
        {
            Debug.Log("GameManager found PlayerController!");
            playerController.OnPlayerStoppedOnLock += OnPlayerStoppedOnLock;
        }
    }

    private void OnPlayerStoppedOnLock ()
    {
        OnChallengeEnd?.Invoke();
    }

    // On Destroy
    private void OnDestroy()
    {
        // On destroying Instance
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            playerController.OnPlayerStoppedOnLock -= OnPlayerStoppedOnLock;
        }
    }
}
