using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class ChallengeEventManager : MonoBehaviour
{
    public struct EventSettings
    {
        public float challengeEventProb;
        public float darkRoomProb;
        public float trippyRoomProb;
    }

    GameDifficulty currentDifficulty = GameDifficulty.Easy;
    Dictionary<GameDifficulty, EventSettings> settings;

    // Dark Room Objects
    [SerializeField] private Light2D globalLight;
    [SerializeField] private GameObject playerSpotLight;
    [SerializeField] private GameObject ambientRoomLight;
    private bool isDarkRoomEventActive = false;

    // Tripping Balls Objects
    [SerializeField] private GameObject trippingBallsVolume;

    void Start()
    {
        GameManager.OnDifficultyChanged += OnDifficultyChanged;
        AnimationManager.OnAnimationAction += OnAnimationAction;
        AnimationManager.Instance.OnAnimationEnd += OnAnimationEnd;
        GameManager.OnChallengeStart += OnChallengeStart;

    }

    private void Awake()
    {
        settings = new Dictionary<GameDifficulty, EventSettings>
        {
            {GameDifficulty.Easy, new EventSettings{ challengeEventProb = 0.2f, darkRoomProb = 0.8f, trippyRoomProb = 0.2f} },
            {GameDifficulty.Medium, new EventSettings{ challengeEventProb = 0.8f, darkRoomProb = 0.75f, trippyRoomProb = 0.25f} },
            {GameDifficulty.Hard, new EventSettings{ challengeEventProb = 0.6f, darkRoomProb = 0.4f, trippyRoomProb = 0.6f} },
            {GameDifficulty.VeryHard, new EventSettings{ challengeEventProb = 0.7f, darkRoomProb = 0.5f, trippyRoomProb = 0.5f} }
        };
    }

    private void OnDifficultyChanged(GameDifficulty difficulty)
    {
        currentDifficulty = difficulty;
    }

    void OnAnimationAction(string action)
    {
        if(action == "OnRoomTransition")
        {
            Debug.Log("Trying for Event");
            TryForEvent();
        }
    }

    private void TryForEvent()
    {
        // Make sure any active event is diabled by start of the room
        DisableAllEvents();

        float probability = settings[currentDifficulty].challengeEventProb;

        if(probability > Random.Range(0.0f, 1.0f))
        {
            Debug.Log("Dark Room Event Activated!");
            RunRandomEvent();
        }
    }

    private void DisableAllEvents()
    {
        DisableDarkRoomEvent();
        DisableTrippingBallsEvent();
    }

    private void RunRandomEvent()
    {
        float darkProb, trippyProb;
        darkProb = settings[currentDifficulty].darkRoomProb;
        trippyProb = settings[currentDifficulty].trippyRoomProb;

        float total = darkProb + trippyProb;
        float rand = Random.Range(0f, total); // Generate a random number between 0 and total

        if (rand < darkProb)
        {
            Debug.Log("Running Dark Event");
            EnableDarkRoomEvent();
        }
        else
        {
            Debug.Log("Running Trippy Event");
            EnableTrippingBallsEvent();
        }
    } 

    private void EnableDarkRoomEvent()
    {
        isDarkRoomEventActive = true;
        globalLight.intensity = 0.02f;
        ambientRoomLight.SetActive(true);
        
    }

    private void DisableDarkRoomEvent()
    {
        isDarkRoomEventActive = false;
        globalLight.intensity = 1f;
        ambientRoomLight.SetActive(false);
    }

    private void EnableTrippingBallsEvent()
    {
        trippingBallsVolume.SetActive(true);
    }

    private void DisableTrippingBallsEvent()
    {
        trippingBallsVolume.SetActive(false);
    }

    private void OnChallengeStart()
    {
        if (isDarkRoomEventActive)
        {
            ambientRoomLight.SetActive(false);
            playerSpotLight.SetActive(true);
        }
    }

    private void OnAnimationEnd(string name)
    {
        if (isDarkRoomEventActive && name == "EvaluateResults")
        {
            ambientRoomLight.SetActive(true);
            playerSpotLight.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnDifficultyChanged -= OnDifficultyChanged;
        AnimationManager.OnAnimationAction -= OnAnimationAction;
        AnimationManager.Instance.OnAnimationEnd -= OnAnimationEnd;
        GameManager.OnChallengeStart -= OnChallengeStart;
    }
}
