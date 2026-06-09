using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharLocomotion : MonoBehaviour
{
    private Rigidbody2D _body;

    private bool _isFacingRight = true;
    public bool IsFacingRight => _isFacingRight;

    [SerializeField] private bool _isStaticEnemy = false;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.linearDamping = 0f;
        _body.angularDamping = 0f;
        
        SetGravityModifier(1f);
    }

    public void Move(float targetSpeed, float acceleration, float direction) 
    {
        if (_isStaticEnemy) return;
        Vector2 targetVelocity = new Vector2(direction * targetSpeed, 0f);
        Accelerate(targetVelocity, acceleration);
    }


    private void Accelerate(Vector2 targetVelocity, float acceleration)
    {
        float xVel = Mathf.MoveTowards(_body.linearVelocityX, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        Vector2 movement = new Vector2(xVel, _body.linearVelocityY);
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
        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;

        float desiredSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y * _body.gravityScale) * (force + 0.3f));
        Vector2 currentHorizontalMovement = new Vector2(_body.linearVelocityX, 0f);

        Vector2 forceVector = direction * desiredSpeed + currentHorizontalMovement;
        _body.linearVelocity = forceVector;
    }

    public void PushByDirectionSimple(Vector2 direction, float force)
    {
        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;
        Vector2 currentHorizontalMovement = new Vector2(_body.linearVelocityX, 0f);
        Vector2 forceVector = direction * force + currentHorizontalMovement;
        _body.linearVelocity = forceVector;
    }

    public void PushByDirectionRaw(Vector2 direction, float force)
    {
        direction = direction.normalized;
        Vector2 forceVector = direction * force;
        _body.linearVelocity = forceVector;
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
