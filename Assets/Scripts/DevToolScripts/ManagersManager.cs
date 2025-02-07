using UnityEngine;

public class ManagersManager : MonoBehaviour
{
    private void Awake()
    {
        // This script ensures that on every scene load that a GameManager is present
        if (GameManager.Instance == null)
        {
            // Dynamically load the SoundManager prefab
            Instantiate(Resources.Load<GameObject>("GameManager"));
        }
    }
}
