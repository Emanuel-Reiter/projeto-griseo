using UnityEngine;

public class PlayerDependencies : MonoBehaviour
{
    public CharacterLocomotion Locomotion { get; private set; }
    public CharacterAnimator CharAnimator { get; private set; }
    public PlayerInputManager Input { get; private set; }
    public CharacterEnvironmentDetection EnvDetection { get; private set; }

    [SerializeField] private CharacterMovmentData _movementData;
    public CharacterMovmentData MovementData => _movementData;

    [SerializeField] private Transform _spellOrigin;
    public Transform SpellOrigin => _spellOrigin;

    private void Awake()
    {
        Locomotion = GetComponent<CharacterLocomotion>();
        CharAnimator = GetComponentInChildren<CharacterAnimator>();
        Input = GetComponent<PlayerInputManager>();
        EnvDetection = GetComponent<CharacterEnvironmentDetection>();
    }
}
