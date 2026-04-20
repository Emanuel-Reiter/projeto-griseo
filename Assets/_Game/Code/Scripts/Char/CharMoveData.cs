using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Character/MovementData")]
public class CharMoveData : ScriptableObject
{
    [Header("Move speed")]
    [SerializeField] private float _walkSpeed = 4f;
    public float WalkSpeed => _walkSpeed;

    [SerializeField] private float _runSpeed = 10f;
    public float RunSpeed => _runSpeed;

    [Header("Acceleration")]
    [SerializeField] private float _baseAcceleration = 1.5f;
    public float BaseAcceleration => _baseAcceleration;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 6f;
    public float JumpHeight => _jumpHeight;
    [SerializeField] private int _maxAmountOfJumps = 1;
    public int MaxAmountOfJumps => _maxAmountOfJumps;

    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplaier = 5.0f;
    public float GravityMultiplaier => _gravityMultiplaier;

    [SerializeField] private float _maxVerticalVel = -50f;
    public float MaxVerticalVel => _maxVerticalVel;
}
