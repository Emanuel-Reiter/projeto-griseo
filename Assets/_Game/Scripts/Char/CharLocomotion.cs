using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(CharEnvDetection))]
public class CharLocomotion : MonoBehaviour
{
    private Rigidbody2D _body;
    private CharEnvDetection _envDetection;
    private CapsuleCollider2D _col;

    private bool _isFacingRight = true;
    public bool IsFacingRight => _isFacingRight;

    [Header("Phys mats")]
    [SerializeField] private PhysicsMaterial2D _fullFrictionMat;
    [SerializeField] private PhysicsMaterial2D _noFrictionMat;

    private bool _isMovingUp = false;
    public bool IsMovingUp => _isMovingUp;
    public void SetIsMovingUp(bool isMovingUp) => _isMovingUp = isMovingUp;

    private void Awake()
    {
        _envDetection = GetComponent<CharEnvDetection>();
        _col = GetComponent<CapsuleCollider2D>();

        _body = GetComponent<Rigidbody2D>();
        _body.linearDamping = 0f;
        _body.angularDamping = 0f;
        
        SetGravityModifier(1f);
    }

    public void Move(float targetSpeed, float acceleration, float direction) 
    {
        if (_envDetection.IsGrounded && !_envDetection.IsOnSlope && !_isMovingUp)
        {
            Vector2 moveVector = new Vector2(direction * targetSpeed, 0f);
            _body.linearVelocity = moveVector;
        }
        else if(_envDetection.IsGrounded && _envDetection.IsOnSlope && !_isMovingUp && _envDetection.IsOnSteepSlope)
        {
            Vector2 moveVector = new Vector2(targetSpeed * _envDetection.SlopeNormalPerp.x * -direction, targetSpeed * _envDetection.SlopeNormalPerp.y * -direction);
            _body.linearVelocity = moveVector;
        }
        else if (!_envDetection.IsGrounded)
        {
            Vector2 moveVector = new Vector2(direction * targetSpeed, _body.linearVelocityY);
            _body.linearVelocity = moveVector;
        }
    }

    private void Update()
    {
        if (_body.linearVelocityY <= 0f) _isMovingUp = false;
    }

    private void FixedUpdate()
    {

    }

    private void Accelerate(Vector2 targetVelocity, float acceleration)
    {
        float xVel = Mathf.MoveTowards(_body.linearVelocityX, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        float yVel = targetVelocity.y;
        Vector2 movement = new Vector2(xVel, yVel);
        _body.linearVelocity = movement;
    }

    public void Decelerate(float acceleration)
    {
        float xVel = Mathf.MoveTowards(_body.linearVelocityX, 0f, acceleration * Time.fixedDeltaTime);
        Vector2 movement = new Vector2(xVel, _body.linearVelocityY);
        _body.linearVelocity = movement;
    }

    public void PushByDirectionComplex(Vector2 direction, float force)
    {
        if (direction.y > 0f) SetIsMovingUp(true);

        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;

        float desiredSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y * _body.gravityScale) * (force + 0.3f));
        Vector2 currentHorizontalMovement = new Vector2(_body.linearVelocityX, 0f);

        Vector2 forceVector = direction * desiredSpeed + currentHorizontalMovement;
        //Debug.Log($"{gameObject.name} moved by: {forceVector}");
        _body.linearVelocity = forceVector;
    }

    public void PushByDirectionSimple(Vector2 direction, float force)
    {
        if (direction.y > 0f) SetIsMovingUp(true);

        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;
        Vector2 currentHorizontalMovement = new Vector2(_body.linearVelocityX, 0f);
        Vector2 forceVector = direction * force + currentHorizontalMovement;
        _body.linearVelocity = forceVector;
    }

    public void PushByDirectionRaw(Vector2 direction, float force)
    {
        if (direction.y > 0f) SetIsMovingUp(true);

        direction = direction.normalized;
        Vector2 forceVector = direction * force;
        _body.linearVelocity += forceVector;
    }

    public void ToggleFriction(bool toggle)
    {
        if (toggle)
        {
            _body.sharedMaterial = _fullFrictionMat;
        }
        else
        {
            _body.sharedMaterial = _noFrictionMat;
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
        if (_isFacingRight && _body.linearVelocityX < 0f)
        {
            transform.localScale = new Vector3(-1f, 1, 1);
            _isFacingRight = false;
        }
        else if (!_isFacingRight && _body.linearVelocityX > 0f)
        {
            transform.localScale = new Vector3(1f, 1, 1);
            _isFacingRight = true;
        }
    }

    public void SetGravityModifier(float modifier) => _body.gravityScale = modifier;


    public Vector2 GetVelocity()
    {
        return _body.linearVelocity;
    }
}
