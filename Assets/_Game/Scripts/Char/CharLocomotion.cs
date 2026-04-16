using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharLocomotion : MonoBehaviour
{
    private Rigidbody2D _rb;

    private bool _isFacingRight = true;
    public bool IsFacingRight => _isFacingRight;

    [Header("Phys mats")]
    [SerializeField] private PhysicsMaterial2D _fullFrictionMat;
    [SerializeField] private PhysicsMaterial2D _noFrictionMat;

    private CharEnvDetection _envDetection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
        
        SetGravityModifier(1f);

        if (!TryGetComponent<CharEnvDetection>(out _envDetection)) Debug.LogError($"No env detection script assinged to: {gameObject.name}.");
    }

    public void Move(float targetSpeed, float acceleration, float direction)
    {
        Vector2 moveVelocity;

        if (_envDetection.OnSlope)
        {
            // Fixed tangent always points rightward along the slope
            Vector2 tangent = _envDetection.SlopeTangentRight;
            Vector2 normal = _envDetection.GroundNormal;

            // Desired movement direction: input sign determines left/right along tangent
            float moveSign = Mathf.Sign(direction);
            if (direction == 0f) moveSign = 0f; // Stop moving if no input

            // Current velocity components along tangent and normal
            float currentTangentSpeed = Vector2.Dot(_rb.linearVelocity, tangent);
            float normalSpeed = Vector2.Dot(_rb.linearVelocity, normal);

            // Accelerate toward target tangent speed
            float targetTangentSpeed = moveSign * targetSpeed;
            float newTangentSpeed = Mathf.MoveTowards(currentTangentSpeed, targetTangentSpeed, acceleration * Time.fixedDeltaTime);

            // Reconstruct velocity: keep normal component (gravity) untouched
            moveVelocity = tangent * newTangentSpeed + normal * normalSpeed;
        }
        else
        {
            // Flat ground: simple horizontal movement
            float targetX = direction * targetSpeed;
            float newX = Mathf.MoveTowards(_rb.linearVelocityX, targetX, acceleration * Time.fixedDeltaTime);
            moveVelocity = new Vector2(newX, _rb.linearVelocityY);
        }

        _rb.linearVelocity = moveVelocity;
    }

    public void Decelerate(float acceleration)
    {
        float newX = Mathf.MoveTowards(_rb.linearVelocityX, 0f, acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = new Vector2(newX, _rb.linearVelocityY);
    }

    public void PushByDirectionComplex(Vector2 direction, float force)
    {
        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;

        float desiredSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale) * (force + 0.3f));
        Vector2 currentHorizontalMovement = new Vector2(_rb.linearVelocityX, 0f);

        Vector2 forceVector = direction * desiredSpeed + currentHorizontalMovement;
        //Debug.Log($"{gameObject.name} moved by: {forceVector}");
        _rb.linearVelocity = forceVector;
    }

    public void PushByDirectionSimple(Vector2 direction, float force)
    {
        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;
        Vector2 currentHorizontalMovement = new Vector2(_rb.linearVelocityX, 0f);
        Vector2 forceVector = direction * force + currentHorizontalMovement;
        _rb.linearVelocity = forceVector;
    }

    public void PushByDirectionRaw(Vector2 direction, float force)
    {
        direction = direction.normalized;
        Vector2 forceVector = direction * force;
        _rb.linearVelocity += forceVector;
    }

    public void ToggleFriction(bool toggle)
    {
        if (toggle)
        {
            _rb.sharedMaterial = _fullFrictionMat;
        }
        else
        {
            _rb.sharedMaterial = _noFrictionMat;
        }
    }

    public void ChangeDirectionByInput(float direction)
    {
        if(_isFacingRight && direction < 0f)
        {
            transform.localScale = new Vector3(-1f, 1, 1);
            _isFacingRight = false;
        } 
        else if (!_isFacingRight && direction > 0f)
        {
            transform.localScale = new Vector3(1f, 1, 1);
            _isFacingRight = true;
        }
    }

    public void ChangeDirectionByVelocity()
    {
        if (_isFacingRight && _rb.linearVelocityX < 0f)
        {
            transform.localScale = new Vector3(-1f, 1, 1);
            _isFacingRight = false;
        }
        else if (!_isFacingRight && _rb.linearVelocityX > 0f)
        {
            transform.localScale = new Vector3(1f, 1, 1);
            _isFacingRight = true;
        }
    }

    public void SetGravityModifier(float modifier) => _rb.gravityScale = modifier;


    public Vector2 GetVelocity()
    {
        return _rb.linearVelocity;
    }
}
