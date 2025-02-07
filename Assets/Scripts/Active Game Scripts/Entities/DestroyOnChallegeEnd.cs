using UnityEngine;

public class DestroyOnChallegeEnd : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnPlayerStoppedOutcome += OnPlayerStopped;
    }

    void OnPlayerStopped(bool outcome)
    {
        Instantiate(destroyVFX, transform.position, destroyVFX.transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerStoppedOutcome -= OnPlayerStopped;
    }
}
