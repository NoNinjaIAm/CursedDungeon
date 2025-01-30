using UnityEngine;

public class LockDestructionHandler : MonoBehaviour
{
    private bool playerPassed = false;
    [SerializeField] private GameObject explosionVFX;
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
                SoundManager.Instance.PlaySound("Lock Broken");
                Instantiate(explosionVFX, transform.position, explosionVFX.transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                SoundManager.Instance.PlaySound("Lock Failed To Break");
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
