using UnityEngine;
using System.Collections;
using System;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public static AnimationManager Instance { get; private set; }

    public event Action OnAnimationEnd;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Prevent duplicates
    }

    public IEnumerator WaitForAnimation(string name)
    {
        // Get the current animation state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentNormTime = stateInfo.normalizedTime;

        // Wait until the animation reaches the end (normalizedTime = 1)
        while (currentNormTime < 1)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // Update stateInfo in the loop
            yield return null;  // Wait for the next frame
            
            // Check after each iteration if we're still in the same state
            if (currentNormTime > stateInfo.normalizedTime) break; // If timer goes lower the current time then new animation has started or this one has reset (and it wasn't caught in conditional)
            currentNormTime = stateInfo.normalizedTime;
        }

        // Animation finished
        Debug.Log("AnimManager Finished Animation. It took: " + stateInfo.length + " seconds");
        OnAnimationEnd?.Invoke();
    }

    public void PlayAnimationAndWait(string animationName)
    {
        animator.Play(animationName);
        StartCoroutine(WaitForAnimation(animationName));
    }

}
