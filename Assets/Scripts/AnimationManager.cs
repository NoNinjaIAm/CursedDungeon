using UnityEngine;
using System.Collections;
using System;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public static AnimationManager Instance { get; private set; }

    public event Action<string> OnAnimationEnd;
    public static event Action<string> OnAnimationAction;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Prevent duplicates
    }

    // Plays an animation and sets a callback for completion
    public void PlayAnimation(string animationName)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
            return;
        }

        animator.Play(animationName);
    }

    // This is called from an Animation Event at the end of the animation
    public void NotifyAnimationEnd(string animationName)
    {
        Debug.Log($"Animation {animationName} finished.");
        OnAnimationEnd?.Invoke(animationName);
    }

    public void FireActionEvent(string eventName)
    {
        OnAnimationAction?.Invoke(eventName);
    }

}
