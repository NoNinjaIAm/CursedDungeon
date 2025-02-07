using UnityEngine;

public class HandleGameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private GameObject roomNumberText;
    [SerializeField] private GameObject roomPlusEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnChallengeStart += OnChallengeStart;
        AnimationManager.OnAnimationAction += OnAnimationAction;
    }

    private void OnAnimationAction(string name)
    {
        if(name == "OnRoomTransition")
        {
            PlayRoomText();
        }
    }

    private void OnGameOver()
    {
        SoundManager.Instance.PlaySound("GameOver");
        gameoverScreen.SetActive(true);

        // Don't gotta listen anymore for event
        GameManager.OnGameOver -= OnGameOver;
    }

    private void PlayRoomText()
    {
        roomNumberText.SetActive(true);
        roomPlusEffect.SetActive(true);
    }

    private void OnChallengeStart()
    {
        roomNumberText.SetActive(false);
        roomPlusEffect.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnChallengeStart -= OnChallengeStart;
        AnimationManager.OnAnimationAction -= OnAnimationAction;
    }


}
