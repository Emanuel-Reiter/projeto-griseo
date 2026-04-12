using UnityEngine;

[RequireComponent (typeof(CharLocomotion))]
public class CharEnvDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool IsFacingWall { get; private set; } = false;
    public bool IsFacingHole { get; private set; } = false;

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _groundDetectionRadius = 0.5f;
    [SerializeField] private float _envDetectionDistance = 1.0f;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
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
    }
#endif
}
