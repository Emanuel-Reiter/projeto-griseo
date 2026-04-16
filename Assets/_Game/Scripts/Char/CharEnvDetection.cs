using UnityEngine;

[RequireComponent (typeof(CharLocomotion))]
public class CharEnvDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool IsFacingWall { get; private set; } = false;
    public bool IsFacingHole { get; private set; } = false;
    public float GroundAngle { get; private set; } = 0f;
    public bool OnSlope { get; private set; } = false;

    public Vector2 GroundNormal { get; private set; } = Vector2.up;
    public Vector2 SlopeTangent { get; private set; } = Vector2.right;
    public Vector2 SlopeTangentRight { get; private set; } = Vector2.right;

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _groundDetectionRadius = 0.5f;
    [SerializeField] private float _envDetectionDistance = 1f;
    [SerializeField] private float _slopeDetectionDistanceForward = 1f;
    [SerializeField] private float _slopeDetectionDistanceDown = 1f;

    private float _slopeAngleMax = 50f;

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
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _groundDetectionRadius, _groundLayers);
        if (hit) return true;
        return false;
    }

    private bool DetectWall()
    {
        float offset = _locomotion.IsFacingRight ? _groundDetectionRadius : -_groundDetectionRadius;
        Vector2 dir = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(transform.position.x + offset, transform.position.y + _envDetectionDistance);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, _envDetectionDistance, _groundLayers);
        if (hit) return true;
        return false;
    }

    private bool DetectHole()
    {
        float offset = _locomotion.IsFacingRight ? _groundDetectionRadius : -_groundDetectionRadius;
        Vector2 origin = new Vector2(transform.position.x + offset, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _envDetectionDistance, _groundLayers);
        if(!IsGrounded) return false;
        if (hit) return false;
        return true;
    }

    private void DetectSlope()
    {
        if (!IsGrounded)
        {
            OnSlope = false;
            GroundAngle = 0f;
            GroundNormal = Vector2.up;
            SlopeTangentRight = Vector2.right;
            return;
        }

        // Use a fixed right direction for tangent calculation
        Vector2 checkDir = Vector2.right;
        RaycastHit2D hitFront = Physics2D.Raycast(transform.position, checkDir, _slopeDetectionDistanceForward, _groundLayers);
        if (hitFront)
        {
            float angle = Vector2.Angle(Vector2.up, hitFront.normal);
            if (angle > 0f && angle < _slopeAngleMax)
            {
                OnSlope = true;
                GroundAngle = angle;
                GroundNormal = hitFront.normal;
                SlopeTangentRight = Vector2.Perpendicular(hitFront.normal).normalized;
                // Ensure tangent points to the right (positive dot with Vector2.right)
                if (Vector2.Dot(SlopeTangentRight, Vector2.right) < 0)
                    SlopeTangentRight = -SlopeTangentRight;
                return;
            }
        }

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, _slopeDetectionDistanceDown, _groundLayers);
        if (hitDown)
        {
            float angle = Vector2.Angle(Vector2.up, hitDown.normal);
            if (angle > 0f && angle < _slopeAngleMax)
            {
                OnSlope = true;
                GroundAngle = angle;
                GroundNormal = hitDown.normal;
                SlopeTangentRight = Vector2.Perpendicular(hitDown.normal).normalized;
                if (Vector2.Dot(SlopeTangentRight, Vector2.right) < 0)
                    SlopeTangentRight = -SlopeTangentRight;
            }
            else
            {
                OnSlope = false;
                GroundAngle = 0f;
                GroundNormal = Vector2.up;
                SlopeTangentRight = Vector2.right;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 dirSlopeOff = Vector2.right;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, dirSlopeOff * _slopeDetectionDistanceForward);
        Gizmos.DrawRay(transform.position, Vector2.down * _slopeDetectionDistanceDown);

        if (!Application.isPlaying) return;

        float offsetWall = _locomotion.IsFacingRight ? _groundDetectionRadius : -_groundDetectionRadius;
        Vector2 originWall = new Vector2(transform.position.x + offsetWall, transform.position.y + _envDetectionDistance);
        Vector2 dirWall = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;

        float offsetHole = _locomotion.IsFacingRight ? _groundDetectionRadius : -_groundDetectionRadius;
        Vector2 originHole = new Vector2(transform.position.x + offsetHole, transform.position.y);


        if (IsGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _groundDetectionRadius);

        if (IsFacingWall) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawRay(originWall, dirWall * _envDetectionDistance);

        if (IsFacingHole) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Gizmos.DrawRay(originHole, Vector2.down * _envDetectionDistance);

        Vector2 dirSlope = _locomotion.IsFacingRight ? Vector2.right : Vector2.left;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, dirSlope * _slopeDetectionDistanceForward);
        Gizmos.DrawRay(transform.position, Vector2.down * _slopeDetectionDistanceDown);
    }
#endif
}
