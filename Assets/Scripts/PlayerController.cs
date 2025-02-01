using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]  private SpinPivot pivotSpinner;
    [SerializeField] private GazeDetector playerGaze;
    [SerializeField] private ChallengeTimer challengeTimer;

    private bool isSpinning = false;

    // If the player stopped on the lock, then this sends "true"
    // If the player stopped but not on the lock, then this sends "false"
    public static event Action<bool> OnPlayerStoppedOutcome;
    
    private void Awake()
    {
        challengeTimer.OnTimesUp += OnTimesUp;
    }
    private void OnEnable()
    {
        SoundManager.Instance.PlayMusic("Tense Music");
        pivotSpinner.StartSpinning();
        isSpinning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isSpinning)
        {
            StopEye();
        }
    }

    void StopEye()
    {
        isSpinning = false;

        // Sound stuff
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySound("Bugs Die");

        pivotSpinner.StopSpinning();
        if (playerGaze.IsLookingAtLock)
        {
            Debug.Log("Stopped On Lock!");
            OnPlayerStoppedOutcome?.Invoke(true); // make sure last thing we do
        }
        else
        {
            Debug.Log("Stopped On Nothing!");
            OnPlayerStoppedOutcome?.Invoke(false); // make sure last thing we do
        }
    }

    private void OnTimesUp()
    {
        StopEye(); // Force Stop Eye
    }

    private void OnDestroy()
    {
        challengeTimer.OnTimesUp -= OnTimesUp;
    }


}
