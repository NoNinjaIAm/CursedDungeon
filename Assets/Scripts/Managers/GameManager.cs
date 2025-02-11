using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEnums;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager Instance { get; private set; }

    [SerializeField] private int score;
    public static int Highscore { get; private set; }
    public static bool enabledTrippyColors;

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

        // Load Save data
        Highscore = PlayerPrefs.GetInt("Highscore", 0); // Default to 0 if not found
        enabledTrippyColors = Convert.ToBoolean(PlayerPrefs.GetInt("EnbabledFlashyColors", 1));
        
        Debug.Log("Loaded Highscore with a value of: " +  Highscore + "And Trippy colors with a value of: " + enabledTrippyColors);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // If we're in the game scene
        if(currentSceneIndex == 1)
        {
            SoundManager.Instance.StopMusic();
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
            SoundManager.Instance.PlayMusic("MenuMusic");
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
        if (score == 5)
        {
            currentDifficulty = GameDifficulty.Medium;
            OnDifficultyChanged?.Invoke(GameDifficulty.Medium);
        }
        else if (score == 12)
        {
            currentDifficulty = GameDifficulty.Hard;
            OnDifficultyChanged?.Invoke(GameDifficulty.Hard);
        }
        else if (score == 17)
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
        if (score > Highscore)
        {
            Highscore = score;
            
            // Save highsore
            PlayerPrefs.SetInt("Highscore", Highscore);
            PlayerPrefs.Save();

            //Testing if saved
            Debug.Log("Testing to see if highscore saved. Highscore laoded value after saving: " + PlayerPrefs.GetInt("Highscore", 0));


        }
    }

    public int GetScore()
    {
        return score;
    }

    public bool GetTrippyColorsOptionEnabled()
    {
        return enabledTrippyColors;
    }
    public void SetTrippyColorsOptionEnabled(bool value)
    {
        enabledTrippyColors = value;
    }

    private void UnsubscribeToGameSceneEvents()
    {
        if(AnimationManager.Instance != null)
        {
            AnimationManager.Instance.OnAnimationEnd -= OnAnimationManagerFinishedAnimation;
        }
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStoppedOutcome;
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
