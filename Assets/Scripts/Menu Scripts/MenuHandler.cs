using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject howToPlayScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OptionsButtons.OnOptionsToggled += OnOptionsToggled;
        HowToPlayButtons.OnHowToPlayToggled += OnHowToToggled;
    }

    private void OnOptionsToggled(bool enabled)
    {
        if (enabled)
        {
            menuScreen.SetActive(false);
            optionsScreen.SetActive(true);
        }
        else
        {
            menuScreen.SetActive(true);
            optionsScreen.SetActive(false);
        }
    }

    private void OnHowToToggled(bool enabled)
    {
        if (enabled)
        {
            menuScreen.SetActive(false);
            howToPlayScreen.SetActive(true);
        }
        else
        {
            menuScreen.SetActive(true);
            howToPlayScreen.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        OptionsButtons.OnOptionsToggled -= OnOptionsToggled;
        HowToPlayButtons.OnHowToPlayToggled -= OnHowToToggled;
    }
}
