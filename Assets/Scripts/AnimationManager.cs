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

    public IEnumerator WaitForAnimation()
    {
        // Get the current animation state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait for the length of the animation
        yield return new WaitForSeconds(stateInfo.length);

        // Animation finished
        Debug.Log("AnimManager Finished Animation");
        OnAnimationEnd?.Invoke();
    }

    public void PlayAnimationAndWait(string animationName)
    {
        animator.Play(animationName);
        StartCoroutine(WaitForAnimation());
    }

}
