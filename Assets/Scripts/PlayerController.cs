using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]  private SpinPivot pivotSpinner;
    [SerializeField] private GazeDetector playerGaze;

    // If the player stopped on the lock, then this sends "true"
    // If the player stopped but not on the lock, then this sends "false"
    public static event Action<bool> OnPlayerStoppedOutcome;

    private void OnEnable()
    {
        pivotSpinner.StartSpinning();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
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
    }

    
}
