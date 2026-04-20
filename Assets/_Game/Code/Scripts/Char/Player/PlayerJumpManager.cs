using UnityEngine;

public class PlayerJumpManager : MonoBehaviour
{
    private bool _isJumpOnCooldown = false;
    private int _jumpsRemaining = 1;    
    private PlayerDependencies _deps;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
        RestoreJumps();
    }

    public bool CanJump()
    {
        return !_isJumpOnCooldown && _jumpsRemaining > 0;
    }

    public void ConsumeJumps()
    {
        _jumpsRemaining--;
        _isJumpOnCooldown = true;
        TimerManager.I.StartTimer(0.1f, () => _isJumpOnCooldown = false);
    }

    public void RestoreJumps()
    {
        _jumpsRemaining = _deps.MoveData.MaxAmountOfJumps;
    }
}
