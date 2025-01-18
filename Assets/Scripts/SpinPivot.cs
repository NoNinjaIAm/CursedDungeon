using UnityEngine;

public class SpinPivot : MonoBehaviour
{
    [SerializeField] private float orbitSpeed = 10f; // Speed of the orbit
    private Vector3 rotationAxis = Vector3.forward;

    private bool isSpinning = true;
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
