using UnityEngine;

public class CharacterEnvironmentDetection : MonoBehaviour
{
    public bool IsGrounded { get; private set; } = false;
    public bool IsFacingWall { get; private set; } = false;

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _groundDetectionRadius = 0.5f;

    void Update()
    {
        IsGrounded = DetectGround();
    }

    private bool DetectGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _groundDetectionRadius, _groundLayers);
        if (hit == null) return false;
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(IsGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, _groundDetectionRadius);
    }
#endif
}
