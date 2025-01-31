using UnityEngine;
using TMPro;

public class HandleRoomNumberText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        text.text = "Room " + GameManager.Instance.GetScore();
    }
}
