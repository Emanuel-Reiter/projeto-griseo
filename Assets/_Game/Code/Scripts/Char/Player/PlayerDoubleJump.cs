using UnityEngine;

public class PlayerDoubleJump : MonoBehaviour
{
    public int JumpsRemaining { get; private set; } = 1;

    public void ConsumeJumps()
    {
        JumpsRemaining--;
    }

    public void RestoreJumps()
    {
        JumpsRemaining = 1;
    }
}
