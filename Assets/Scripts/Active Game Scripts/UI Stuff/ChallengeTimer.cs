using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ChallengeTimer : MonoBehaviour
{
    public float startTime = 10f; // Countdown start time in seconds
    private float currentTime; // Current countdown time
    private bool timerActive = false;
    public TextMeshProUGUI timerText; // Reference to TextMeshPro UI

    public event Action OnTimesUp;
    // public Text timerText; // Uncomment this if using standard Text UI

    private void Start()
    {
        GameManager.OnChallengeStart += StartTimer;
        PlayerController.OnPlayerStoppedOutcome += CancelTimer;
    }

    private void StartTimer()
    {
        timerText.gameObject.SetActive(true); // Show timer UI
        currentTime = startTime; // Initialize timer
        UpdateTimerDisplay();   // Update UI at the start
        timerActive = true; // Timer is set to active
    }

    private void Update()
    {
        // If the timer is active
        if (timerActive)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // Decrease time
                currentTime = Mathf.Max(currentTime, 0); // Clamp to 0
                UpdateTimerDisplay(); // Update the UI
            }
            else
            {
                TimerEnded(); // Handle what happens when the timer hits 0
            }
        } 
    }

    private void UpdateTimerDisplay()
    {
        // Display the time as whole seconds
        timerText.text = "Time Left: " + Mathf.CeilToInt(currentTime).ToString();
    }

    private void TimerEnded()
    {
        // Event here
        OnTimesUp?.Invoke();
    }

    private void CancelTimer (bool outcome)
    {
        timerActive = false; // Timer is no longer active
        timerText.gameObject.SetActive(false); // Deactivate Timer
    }

    private void OnDestroy()
    {
        GameManager.OnChallengeStart -= StartTimer;
        PlayerController.OnPlayerStoppedOutcome -= CancelTimer;
    }
}
