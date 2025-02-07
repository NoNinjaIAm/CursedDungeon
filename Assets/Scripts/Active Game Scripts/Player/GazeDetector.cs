using UnityEngine;

public class GazeDetector : MonoBehaviour
{
    public bool IsLookingAtLock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lock"))
        {
            IsLookingAtLock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lock"))
        {
            IsLookingAtLock = false;
        }
    }
}
