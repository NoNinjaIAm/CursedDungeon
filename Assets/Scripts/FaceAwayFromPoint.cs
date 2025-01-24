using UnityEngine;

public class FaceAwayFromPoint : MonoBehaviour
{
    [SerializeField] private GameObject pointToLookAwayFrom;

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = pointToLookAwayFrom.transform.position;
        Vector3 start = gameObject.transform.position;

        gameObject.transform.up = (start - destination).normalized;
    }
}
