using UnityEngine;

public class LockDestructionHandler : MonoBehaviour
{
    private bool playerPassed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStopped;
        AnimationManager.OnAnimationAction += OnAnimationAction;
    }

    private void OnPlayerStopped(bool outcome)
    {
        playerPassed = outcome;
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
    }

    private void OnAnimationAction(string action)
    {
        if(action == "TryKillLock")
        {
            if(playerPassed)
            {
                // To do effects
                Debug.Log("Killing Lock");
                Destroy(gameObject);
            }

            AnimationManager.OnAnimationAction -= OnAnimationAction;
        }
    }

    private void OnDestroy()
    {
        AnimationManager.OnAnimationAction -= OnAnimationAction;
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
    }
}
