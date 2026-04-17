using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharLocomotion))]
public class CharEnvDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool IsFacingWall { get; private set; } = false;
    public bool IsFacingHole { get; private set; } = false;
    public bool IsOnSlope { get; private set; } = false;
    public bool IsOnSteepSlope { get; private set; } = false;

    [Header("Ground detection")]
    [SerializeField] private LayerMask _groundLayers;

    [SerializeField] private float _groundDetectionRadius = 0.5f;
    [SerializeField] private Vector2 _groundDetectionOffset = Vector2.zero;

    [Header("Slope detection")]
    [SerializeField] private float _slopeDetectDistance = 1f;

    [Header("Walls and holes detection")]
    [SerializeField] private float _envDetectionDistance = 1f;
    [SerializeField] private Transform _holeDetectionOrigin;
    [SerializeField] private Transform _wallDetectionTranform;

    private Vector2 _slopeNormalPerp;
    public Vector2 SlopeNormalPerp => _slopeNormalPerp;

    private float _previousSlopeDownAngle;
    private float _slopeDownAngle;

    private float _previousSlopeHorizontalAngle;
    private float _slopeHorizontalAngle;

    private float _slopeAngleMax = 44f;

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
        DetectSlope();
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

    private void DetectSlope()
    {
        Vector2 detectPos = transform.position;

        CheckSlopesHorizontal(detectPos);
        CheckSlopesVertical(detectPos);

        if (_slopeDownAngle > _slopeAngleMax || _slopeHorizontalAngle > _slopeAngleMax) IsOnSteepSlope = true;
        IsOnSteepSlope = false;
    }

    private void CheckSlopesHorizontal(Vector2 pos)
    {
        Vector2 right = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 left = _locomotion.IsFacingRight ? Vector2.left : Vector2.right;

        RaycastHit2D hitFront = Physics2D.Raycast(pos, right, _slopeDetectDistance, _groundLayers);
        RaycastHit2D hitBack = Physics2D.Raycast(pos, left, _slopeDetectDistance, _groundLayers);

        if (hitFront)
        {
            IsOnSlope = true;
            _slopeHorizontalAngle = Vector2.Angle(hitFront.normal, Vector2.up);
        }
        else if (hitBack)
        {
            IsOnSlope = true;
            _slopeHorizontalAngle = Vector2.Angle(hitBack.normal, Vector2.up);
        }
        else
        {
            IsOnSlope = false;
            _slopeHorizontalAngle = 0f;
        }
    }

    private void CheckSlopesVertical(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, _slopeDetectDistance, _groundLayers);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);

            if (_slopeDownAngle != _previousSlopeDownAngle)
            {
                IsOnSlope = true;
            }

            _previousSlopeDownAngle = _slopeDownAngle;

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            _slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
        }
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
