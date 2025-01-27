using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject optionsScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OptionsButtons.OnOptionsToggled += OnOptionsToggled;
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

    private void OnDestroy()
    {
        OptionsButtons.OnOptionsToggled -= OnOptionsToggled;
    }
}
