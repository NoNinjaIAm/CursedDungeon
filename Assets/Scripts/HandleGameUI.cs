using UnityEngine;

public class HandleGameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameoverScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStopped;
    }

    private void OnPlayerStopped(bool outcome)
    {
        if(!outcome)
        {
            gameoverScreen.SetActive(true);

            // Don't gotta listen anymore for event
            PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
    }
}
