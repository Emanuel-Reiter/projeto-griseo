using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Character/MovementData")]
public class CharacterMovmentData : ScriptableObject
{
    [Header("Move speed")]
    [SerializeField] private float _walkSpeed = 4f;
    public float WalkSpeed => _walkSpeed;

    [SerializeField] private float _runSpeed = 10f;
    public float RunSpeed => _runSpeed;

    [Header("Acceleration")]
    [SerializeField] private float _baseAcceleration = 2f;
    public float BaseAcceleration => _baseAcceleration;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 5f;
    public float JumpHeight => _jumpHeight;

    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplaier = 4.0f;
    public float GravityMultiplaier => _gravityMultiplaier;

    [SerializeField] private float _maxVerticalVel = -30f;
    public float MaxVerticalVel => _maxVerticalVel;
}
