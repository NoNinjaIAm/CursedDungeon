using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        text.text = "Highscore: " + GameManager.Highscore;
    }
}
