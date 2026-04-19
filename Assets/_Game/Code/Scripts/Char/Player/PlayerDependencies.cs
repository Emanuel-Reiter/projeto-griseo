using UnityEngine;

public class PlayerDependencies : MonoBehaviour
{
    public CharLocomotion Locomotion { get; private set; }
    public CharAnimator CharAnimator { get; private set; }
    public PlayerInputManager Input { get; private set; }
    public CharEnvDetection EnvDetection { get; private set; }

    [SerializeField] private CharMoveData _moveData;
    public CharMoveData MoveData => _moveData;

    [SerializeField] private Transform _spellOrigin;
    public Transform SpellOrigin => _spellOrigin;

    public ProjectileCaster Cast { get; private set; }
    public PlayerEquipment Equipment { get; private set; }
    public PlayerAnimationEvents AnimationEvents { get; private set; }

    private void Awake()
    {
        Locomotion = GetComponent<CharLocomotion>();
        CharAnimator = GetComponentInChildren<CharAnimator>();
        Input = GetComponent<PlayerInputManager>();
        EnvDetection = GetComponent<CharEnvDetection>();
        Cast = GetComponent<ProjectileCaster>();
        Equipment = GetComponent<PlayerEquipment>();
        AnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
}
