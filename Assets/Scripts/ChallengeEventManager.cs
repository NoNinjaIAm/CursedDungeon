using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class ChallengeEventManager : MonoBehaviour
{
    GameDifficulty currentDifficulty = GameDifficulty.Easy;
    Dictionary<GameDifficulty, float> settings;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private GameObject playerSpotLight;
    [SerializeField] private GameObject ambientRoomLight;

    private bool isDarkRoomEventActive = false;
    void Start()
    {
        GameManager.OnDifficultyChanged += OnDifficultyChanged;
        AnimationManager.OnAnimationAction += OnAnimationAction;
        AnimationManager.Instance.OnAnimationEnd += OnAnimationEnd;
        GameManager.OnChallengeStart += OnChallengeStart;

    }

    private void Awake()
    {
        settings = new Dictionary<GameDifficulty, float>
        {
            {GameDifficulty.Easy, .2f },
            {GameDifficulty.Medium, .2f },
            {GameDifficulty.Hard, .35f },
            {GameDifficulty.VeryHard, .6f }
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
        float probability = settings[currentDifficulty];

        if(probability > Random.Range(0.0f, 1.0f))
        {
            Debug.Log("Dark Room Event Activated!");
            EnableDarkRoomEvent();
        }
        else
        {
            Debug.Log("Dark Room Event Deactivated!");
            DisableDarkRoomEvent();
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
