using UnityEngine;

public class HandleGameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameoverScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        gameoverScreen.SetActive(true);

        // Don't gotta listen anymore for event
        GameManager.OnGameOver -= OnGameOver;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
    }
}
