using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 1;

    public void ChangeScene()
    {
        SoundManager.Instance.PlaySound("Transition Button");
        SceneManager.LoadScene(sceneIndex);
    }
}
