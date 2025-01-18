using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    private SpinPivot pivotSpinner;
    [SerializeField] private GazeDetector playerGaze;

    public event Action OnPlayerStoppedOnLock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivotSpinner = GetComponent<SpinPivot>();
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
                OnPlayerStoppedOnLock?.Invoke();
            }
            else
            {
                Debug.Log("Stopped On Nothing!");
            }
        }
    }

    
}
