using UnityEngine;

public class HaveRandomSize : MonoBehaviour
{
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.localScale *= Random.Range(minScale, maxScale);
    }
}
