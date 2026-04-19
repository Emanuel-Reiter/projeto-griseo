using UnityEngine;

[RequireComponent (typeof(Animator))]
public class CharAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void PlayInterpolated(AnimationClip animation, float interpolationTime) {
        _animator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }

    public void Play(AnimationClip animation) {
        _animator.Play(animation.name);
    }

    public void SetFloat(string paramName, float paramValue)
    {
        _animator.SetFloat(paramName, paramValue);
    }
}
