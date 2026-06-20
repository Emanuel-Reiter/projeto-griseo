using UnityEngine;

[RequireComponent(typeof(CharLocomotion))]
public class CharEnvDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool IsFacingWall { get; private set; } = false;
    public bool IsFacingHole { get; private set; } = false;

    [Header("Ground detection")]
    [SerializeField] private LayerMask _groundLayers;

    [SerializeField] private float _groundDetectionRadius = 0.5f;
    [SerializeField] private Vector2 _groundDetectionOffset = Vector2.zero;

    [Header("Walls and holes detection")]
    [SerializeField] private float _envDetectionDistance = 1f;
    [SerializeField] private Transform _holeDetectionOrigin;
    [SerializeField] private Transform _wallDetectionTranform;

    private CharLocomotion _locomotion;

    private void Awake()
    {
        _locomotion = GetComponent<CharLocomotion>();
    }

    void Update()
    {
        IsGrounded = DetectGround();
        IsFacingWall = DetectWall();
        IsFacingHole = DetectHole();
    }

    private bool DetectGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position + (Vector3)_groundDetectionOffset, _groundDetectionRadius, _groundLayers);

        if (hit) return true;
        return false;
    }

    private bool DetectWall()
    {
        Vector2 dir = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(_wallDetectionTranform.position, dir, _envDetectionDistance, _groundLayers);
        if (hit) return true;
        return false;
    }

    private bool DetectHole()
    {
        RaycastHit2D hit = Physics2D.Raycast(_holeDetectionOrigin.position, Vector2.down, _envDetectionDistance, _groundLayers);
        if (!IsGrounded) return false;
        if (hit) return false;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (IsGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)_groundDetectionOffset, _groundDetectionRadius);

        if (!Application.isPlaying) return;

        Vector2 dirWall = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;
        if (IsFacingWall) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawRay(_wallDetectionTranform.position, dirWall * _envDetectionDistance);

        if (IsFacingHole) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawRay(_holeDetectionOrigin.position, Vector2.down * _envDetectionDistance);
    }
#endif
}
