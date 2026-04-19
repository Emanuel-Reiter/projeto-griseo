using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour {
    public bool IsStateComplete { get; protected set; } = false;

    [Header("Base animation")]
    [SerializeField] protected AnimationClip _baseAnim;
    public AnimationClip GetBaseAnim() => _baseAnim;


    [Header("Exit params")]
    [SerializeField] private bool _hasExitTime = false;
    public bool HasCompletedExitTime()
    {
        if (_hasExitTime) return _currentTime >= _duration;
        else return true;
    }

    [SerializeField] private float _duration = 1.0f;
    public float GetStateDuration() => _duration;
    public void SetStateDuration(float duration) { _duration = duration; }
    public float GetStateCompletion(float percentage) => GetStateDuration() * Mathf.Clamp01(percentage);


    private float _startTime;
    private float _currentTime => Time.time - _startTime;
    public float GetCurrentTime() => _currentTime;


    public abstract void CheckExitState(PlayerStateManager manager);
    public abstract void EnterState(PlayerStateManager manager);
    public abstract void UpdateState(PlayerStateManager manager);
    public abstract void PhysicsUpdateState(PlayerStateManager manager);
    public abstract void ExitState(PlayerStateManager manager);

    public void InitializeState()
    {
        _startTime = Time.time;
        IsStateComplete = false;
    }

}