using UnityEngine;

public class SpinPivot : MonoBehaviour
{
    [SerializeField] private float orbitSpeed = 10f; // Speed of the orbit
    private bool isSpinning = false;
    
    private Vector3 rotationAxis = Vector3.forward;
    private Quaternion startRot;

    private void Start()
    {
        startRot = transform.localRotation;
    }

    // When gameObject is turned back on we reset rotation position
    private void OnEnable()
    {
        transform.localRotation = startRot;
    }

    void Update()
    {
        // Rotate the pivot
        if(isSpinning)
        {
            transform.Rotate(rotationAxis, orbitSpeed * Time.deltaTime);
        }
    }

    public void StopSpinning ()
    {
        isSpinning = false;
    }

    public void StartSpinning()
    {
        isSpinning = true;
    }

}
