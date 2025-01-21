using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private int gameIndex = 1;
    public void StartGame()
    {
        SceneManager.LoadScene(gameIndex);
    }
}
