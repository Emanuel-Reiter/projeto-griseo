using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterLocomotion : MonoBehaviour
{
    private Rigidbody2D _rb;

    private bool _isFacingRight = true;
    public bool IsFacingRight => _isFacingRight;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
        
        SetGravityModifier(1f);
    }

    public void Move(float targetSpeed, float acceleration, Vector2 direction) 
    {
        Vector2 moveVector = new Vector2(direction.x * targetSpeed, 0f);
        Accelerate(moveVector, acceleration);
    }

    private void Accelerate(Vector2 targetVelocity, float acceleration)
    {
        float xVel = Mathf.MoveTowards(_rb.linearVelocityX, targetVelocity.x, acceleration);
        Vector2 movement = new Vector2(xVel, _rb.linearVelocityY);
        _rb.linearVelocity = movement;
    }

    public void Decelerate(float acceleration)
    {
        float xVel = Mathf.MoveTowards(_rb.linearVelocityX, 0f, acceleration);
        Vector2 movement = new Vector2(xVel, _rb.linearVelocityY);
        _rb.linearVelocity = movement;
    }

    public void PushByDirection(Vector2 direction, float force)
    {
        if (!_isFacingRight) direction = new Vector2(-direction.x, direction.y);

        direction = direction.normalized;

        float desiredSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale) * (force + 0.3f));
        Vector2 currentHorizontalMovement = new Vector2(_rb.linearVelocityX, 0f);

        Vector2 forceVector = direction * desiredSpeed + currentHorizontalMovement;
        _rb.linearVelocity = forceVector;
    }

    public void ChangeDirectionByInput(Vector2 direction)
    {
        if(_isFacingRight && direction.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1, 1);
            _isFacingRight = false;
        } 
        else if (!_isFacingRight && direction.x > 0f)
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
