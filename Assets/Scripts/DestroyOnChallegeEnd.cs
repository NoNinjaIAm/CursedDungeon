using UnityEngine;

public class DestroyOnChallegeEnd : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStopped;
    }

    void OnPlayerStopped(bool outcome)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
    }
}
