using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager Instance { get; private set; }
    private bool waitingForStartTransAnimation = false;
    private bool waitingForChallengeTransAnimation = false;

    [SerializeField] private int score;
    public static event Action OnChallengeStart;

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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // If we're in the game scene
        if(currentSceneIndex == 1)
        {
            FindPlayerController(); // Gotta find PlayerController on every game scene reload
            playerController.gameObject.SetActive(false);

            // Play and wait for starting game animation to finish
            if (AnimationManager.Instance != null)
            {
                // Subscribe to Animation Listener event
                AnimationManager.Instance.OnAnimationEnd += OnCutsceneFinished;

                // Play start game animation and wait for it to end
                waitingForStartTransAnimation = true;
                AnimationManager.Instance.PlayAnimationAndWait("StartGameTransition");
            }
            else Debug.LogError("ERROR: GAMEMANAGER CANNOT FIND ANIMATIONMANAGER INSTANCE");

            
        }
        
    }

    private void OnCutsceneFinished()
    {
        // If the opening cutscene just finisihed
        if(waitingForStartTransAnimation)
        {
            waitingForStartTransAnimation=false;
            StartChallenge();
        }
        else if(waitingForChallengeTransAnimation)
        {
            waitingForChallengeTransAnimation=false;
            StartChallenge();
        }
        else
        {
            Debug.LogWarning("Some Animation Finished and GameManager did not account for it.");
        }
    }

    private void FindPlayerController()
    {
        // Find the PlayerController in the new scene
        playerController = FindFirstObjectByType<PlayerController>();
        
        if (playerController == null) Debug.LogError("ERROR: Player Controller Not Found By GameManager!");
        else
        {
            Debug.Log("GameManager found PlayerController!");
            PlayerController.OnPlayerStoppedOutcome += OnPlayerStoppedOutcome;
        }
    }

    private void StartChallenge()
    {
        // Activate Game
        playerController.gameObject.SetActive(true);
        OnChallengeStart();
    }

    private void OnPlayerStoppedOutcome (bool outcome)
    {
        // If passed challenge
        if (outcome)
        {
            // Deactivate Game
            playerController.gameObject.SetActive(false);
            score++;
            Debug.Log("Challenge Passed. New Score: " + score);
            
            // Play cutscene and wait
            AnimationManager.Instance.PlayAnimationAndWait("LevelTransition");
            waitingForChallengeTransAnimation = true;
        }
        // If failed challenge
        else
        {
            Debug.Log("Game Over!!!");

            playerController.gameObject.SetActive(false);
            UnsubscribeToGameSceneEvents();
            // TODO: Highscore stuff
            AnimationManager.Instance.PlayAnimationAndWait("ChallengeFailed");
        }
        
    }

    private void UnsubscribeToGameSceneEvents()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
        AnimationManager.Instance.OnAnimationEnd -= OnCutsceneFinished;
    }

    // On Destroy
    private void OnDestroy()
    {
        // On destroying Instance
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UnsubscribeToGameSceneEvents();
        }
    }
}
