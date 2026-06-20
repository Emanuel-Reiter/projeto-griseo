using UnityEngine;
using System;
using System.Collections.Generic;

public class TimerManager : Singleton<TimerManager>
{
    private Dictionary<int, TimerData> _activeTimers = new Dictionary<int, TimerData>();
    private int _nextTimerId = 1;

    private readonly List<int> _completedTimers = new List<int>(64);
    private readonly List<Action> _pendingCallbacks = new List<Action>(64);

    private class TimerData
    {
        public float Duration;
        public float TimeRemaining;
        public Action Callback;
        public bool IsActive = true;
    }

    public int StartTimer(float duration, Action callback)
    {
        var timerData = new TimerData
        {
            Duration = duration,
            TimeRemaining = duration,
            Callback = callback
        };

        int timerId = _nextTimerId++;
        _activeTimers.Add(timerId, timerData);
        return timerId;
    }

    public bool IsTimerComplete(int timerId)
    {
        if (!_activeTimers.TryGetValue(timerId, out TimerData timer))
        {
            return true;
        }
        return !timer.IsActive;
    }

    public void CancelTimer(int timerId)
    {
        if (_activeTimers.ContainsKey(timerId))
        {
            _activeTimers.Remove(timerId);
        }
    }

    public float GetRemainingTime(int timerId)
    {
        if (_activeTimers.TryGetValue(timerId, out TimerData timer) && timer.IsActive)
        {
            return timer.TimeRemaining;
        }
        return 0.0f;
    }

    public float GetProgress(int timerId)
    {
        if (_activeTimers.TryGetValue(timerId, out TimerData timer) && timer.IsActive)
        {
            return 1 - (timer.TimeRemaining / timer.Duration);
        }
        return 1.0f;
    }

    private void Update()
    {
        _completedTimers.Clear();
        _pendingCallbacks.Clear();

        foreach (var kvp in _activeTimers)
        {
            int timerId = kvp.Key;
            TimerData timer = kvp.Value;

            if (!timer.IsActive) continue;

            timer.TimeRemaining -= Time.deltaTime;

            if (timer.TimeRemaining <= 0f)
            {
                timer.TimeRemaining = 0f;
                timer.IsActive = false;

                if (timer.Callback != null)
                    _pendingCallbacks.Add(timer.Callback);

                _completedTimers.Add(timerId);
            }
        }

        for (int i = 0; i < _completedTimers.Count; i++)
            _activeTimers.Remove(_completedTimers[i]);

        for (int i = 0; i < _pendingCallbacks.Count; i++)
            _pendingCallbacks[i]?.Invoke();
    }
}