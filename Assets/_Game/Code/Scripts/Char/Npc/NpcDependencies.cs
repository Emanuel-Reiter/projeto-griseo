using UnityEngine;

public class NpcDependencies : MonoBehaviour
{
    public CharLocomotion Locomotion { get; private set; }
    public CharAnimator CharAnimator { get; private set; }
    public CharEnvDetection EnvDetection { get; private set; }

    [SerializeField] private CharMoveData _moveData;
    public CharMoveData MoveData => _moveData;

    [SerializeField] private Transform _spellOrigin;
    public Transform SpellOrigin => _spellOrigin;
    public NpcTargetDetection TargetDetection { get; private set; }

    public SpellProjectileCaster Cast { get; private set; }
    public NpcEquipment Equipment { get; private set; }
    public CharAttributesManager Attributes { get; private set; }

    private void Awake()
    {
        Locomotion = GetComponent<CharLocomotion>();
        CharAnimator = GetComponentInChildren<CharAnimator>();
        EnvDetection = GetComponent<CharEnvDetection>();
        Cast = GetComponent<SpellProjectileCaster>();
        TargetDetection = GetComponent<NpcTargetDetection>();
        Equipment = GetComponent<NpcEquipment>();
        Attributes = GetComponent<CharAttributesManager>();
    }
}
