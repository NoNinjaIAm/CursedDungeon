using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static GameEnums;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager Instance { get; private set; }

    [SerializeField] private int score;
    [SerializeField] private int highscore;

    // Events
    public static event Action OnChallengeStart;
    public static event Action OnGameOver; // Tells when game is lost
    public static event Action<GameDifficulty> OnDifficultyChanged;

    private bool PlayerOutcomeStatusTemp;
    private GameDifficulty currentDifficulty;

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
            // Reset Score
            score = 0;

            FindPlayerController(); // Gotta find PlayerController on every game scene reload
            playerController.gameObject.SetActive(false);

            // Play and wait for starting game animation to finish
            if (AnimationManager.Instance != null)
            {
                // Subscribe to Animation Listener event
                AnimationManager.Instance.OnAnimationEnd += OnAnimationManagerFinishedAnimation;

                // Play start game animation and wait for it to end
                AnimationManager.Instance.PlayAnimation("StartGameTransition");
            }
            else Debug.LogError("ERROR: GAMEMANAGER CANNOT FIND ANIMATIONMANAGER INSTANCE");

            // Set Game Initial Difficukty
            currentDifficulty = GameDifficulty.Easy;
            OnDifficultyChanged?.Invoke(currentDifficulty);
        }
        else // In Menu
        {
            SoundManager.instance.PlayMusic();
        }
        
    }

    private void OnAnimationManagerFinishedAnimation(string name)
    {
        // If the opening cutscene just finisihed
        if(name == "StartGameTransition" || name == "LevelTransition")
        {
            StartChallenge();
        }
        else if(name == "EvaluateResults")
        {
            ProcessChallengeResults();
        }
        else
        {
            Debug.LogWarning("Some Animation Finished and GameManager did not account for it.");
        }
    }

    private void ProcessChallengeResults()
    {
        if (PlayerOutcomeStatusTemp)
        {
            // Increase Score
            HandleScoreChange();

            // Deactivate Game
            playerController.gameObject.SetActive(false);
            Debug.Log("Challenge Passed. New Score: " + score);

            // Play cutscene and wait
            AnimationManager.Instance.PlayAnimation("LevelTransition");
        }
        // If failed challenge
        else
        {
            // Invoke GameOver event
            OnGameOver?.Invoke();
            Debug.Log("Game Over!!!");

            //playerController.gameObject.SetActive(false);
            UnsubscribeToGameSceneEvents();
            // TODO: Highscore stuff
            AnimationManager.Instance.PlayAnimation("ChallengeFailed");
            CheckHighscore();
        }
    }

    private void HandleScoreChange()
    {
        score++;
        if (score == 3)
        {
            currentDifficulty = GameDifficulty.Medium;
            OnDifficultyChanged?.Invoke(GameDifficulty.Medium);
        }
        else if (score == 6)
        {
            currentDifficulty = GameDifficulty.Hard;
            OnDifficultyChanged?.Invoke(GameDifficulty.Hard);
        }
        else if (score == 10)
        {
            currentDifficulty = GameDifficulty.VeryHard;
            OnDifficultyChanged?.Invoke(GameDifficulty.VeryHard);
        }
        Debug.Log("Difficulty Is: " + currentDifficulty);
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
        OnChallengeStart?.Invoke();
    }

    private void OnPlayerStoppedOutcome (bool outcome)
    {
        PlayerOutcomeStatusTemp = outcome;
        AnimationManager.Instance.PlayAnimation("EvaluateResults");
    }

    private void CheckHighscore ()
    {
        if (score > highscore)
        {
            highscore = score;
            Debug.Log("New Highscore: " + highscore);
        }
    }

    private void UnsubscribeToGameSceneEvents()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
        AnimationManager.Instance.OnAnimationEnd -= OnAnimationManagerFinishedAnimation;
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
