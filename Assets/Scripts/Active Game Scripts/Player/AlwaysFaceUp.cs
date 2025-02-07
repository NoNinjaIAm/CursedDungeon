using UnityEngine;

public class AlwaysFaceUp : MonoBehaviour
{
    void Update()
    {
        Vector3 destination = Vector3.down;
        Vector3 start = gameObject.transform.position;

        gameObject.transform.up = (start - destination).normalized;
    }
}
